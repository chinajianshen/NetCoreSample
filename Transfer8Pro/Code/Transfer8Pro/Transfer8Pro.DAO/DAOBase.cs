using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Transfer8Pro.DAO.DataHandlers;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.DAO
{
    public class DAOBase : DAOBase<OracleConnection>
    {
        public DAOBase()
            : base(OracleHelper.MyConnectStr)
        {

        }

        public DataTable QueryData_PerPage(ParamtersForDBPageEntity info, out int totalcnt)
        {
            if (info.UserPagination)
            {
                int records = 0;
                DataTable rt = ExecuteFor(c =>
                {
                    OracleDynamicParameters p = new OracleDynamicParameters();
                    p.Add("TableName", OracleDbType.Varchar2, ParameterDirection.Input, info.TableName);
                    p.Add("Fields", OracleDbType.Varchar2, ParameterDirection.Input, info.Fields);
                    p.Add("OrderField", OracleDbType.Varchar2, ParameterDirection.Input, info.OrderField);
                    p.Add("sqlWhere", OracleDbType.Varchar2, ParameterDirection.Input, info.SqlWhere);
                    p.Add("pageSize", OracleDbType.Int32, ParameterDirection.Input, info.PageSize);
                    p.Add("pageIndex", OracleDbType.Int32, ParameterDirection.Input, info.PageIndex);
                    p.Add("Records", OracleDbType.Int32, ParameterDirection.Output);
                    p.Add("o_resultSet", OracleDbType.RefCursor, ParameterDirection.Output);
                    DataTable dt = c.QueryDT("sp_page", param: p, commandType: CommandType.StoredProcedure);
                    records = p.Get<int>("Records");
                    return dt;
                });

                totalcnt = records;
                return rt;
            }
            else
            {
                string sql = string.Format("select {0} from {1} where {2} order by {3}", info.Fields, info.TableName, info.SqlWhere, info.OrderField);
                DataTable dt = ExecuteFor(c =>
                {
                    return c.QueryDT(sql);
                });
                totalcnt = dt.Rows.Count;
                return dt;
            }
        }
    }


    public class DAOBase<D> where D : DbConnection, new()
    {

        public DAOBase()
        {

        }
        public DAOBase(string default_connect_str)
        {
            _default_connect_str = default_connect_str;
        }
        protected string _default_connect_str = "";

        protected T ExecuteFor<T>(Func<IDbConnection, T> act, string connect_string = null)
        {
            try
            {
                using (IDbConnection con = new D() { ConnectionString = connect_string ?? _default_connect_str })
                {
                    return act(con);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return default(T);
            }
        }

        protected void ExecuteFor(Action<IDbConnection> act, string connect_string = null)
        {
            try
            {
                using (IDbConnection con = new D() { ConnectionString = connect_string ?? _default_connect_str })
                {
                    act(con);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
        }

        protected bool ExecuteForWithTrans(Func<IDbTransaction, bool> act, string connect_string = null)
        {
            IDbTransaction trans = null;
            try
            {
                using (IDbConnection con = new D() { ConnectionString = connect_string ?? _default_connect_str })
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    if (act(trans))
                    {
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return false;
            }
        }


        protected T ExecuteForWithTrans<T>(Func<IDbTransaction, T> act, Func<T, bool> handleRtFunc, string connect_string = null)
        {
            IDbTransaction trans = null;
            try
            {
                using (IDbConnection con = new D() { ConnectionString = connect_string ?? _default_connect_str })
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    T rt = act(trans);

                    if (handleRtFunc(rt))
                    {
                        trans.Commit();
                        return rt;
                    }
                    else
                    {
                        trans.Rollback();
                        return rt;
                    }
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                LogUtil.WriteLog(ex);
                return default(T);
            }
        }
       
        protected virtual string GetSqliteConnectString()
        {
            string localDbConn = ConfigHelper.GetDBConnectStringConfig("LocalDBConnectStr");
            string localDbPath;

            if (!RegexExpressionUtil.ValidateFilePathReg.IsMatch(ConfigHelper.GetConfig("SQLitePath")))
            {
                localDbPath = Path.Combine(AppPath.App_Root, ConfigHelper.GetConfig("SQLitePath"));
            }
            else
            {
                localDbPath = ConfigHelper.GetConfig("SQLitePath");
            }
            localDbConn = string.Format(localDbConn, localDbPath);
            return localDbConn;
        }
    }


    public class DataHandler_DAOBase<D> : DAOBase<D>, IDataHandler where D : DbConnection, new()
    {
        public DataHandler_DAOBase()
            : base()
        {

        }
        public DataHandler_DAOBase(string default_connect_str)
            : base(default_connect_str)
        {

        }
        public bool OutputData(string sql, DateTime op, DateTime ed, string target_jl_file, string connect_string = null)
        {

            ExecuteFor(c =>
            {
                using (StreamWriter sw = new StreamWriter(target_jl_file, false, Encoding.UTF8))
                {
                    c.QueryRawDataReader(r =>
                    {
                        int filedcnt = r.FieldCount;
                        Dictionary<string, object> line = new Dictionary<string, object>();
                        for (int i = 0; i < filedcnt; i++)
                        {
                            line.Add(r.GetName(i), r.GetValue(i));
                        }

                        sw.WriteLine(string.Format("{0}__END_OF_JSON__", JsonObj<Dictionary<string, object>>.ToJson(line)));

                    }, sql, new { StartTime = op, EndTime = ed });
                }
            }, connect_string);
            return true;
        }

        public bool OutputDataToSQLite(string sql, DateTime op, DateTime ed, string sqliteFileFullpath, string connect_string = null)
        {
            DataFieldTypeCollection dataFieldTypes = GetDataFieldCollection(sql);
            string sqliteConnString = SQLiteHelper.CreateSQLiteFileConnectionString(sqliteFileFullpath);
            SQLiteHelper.CreateDataBase(sqliteConnString, sqliteFileFullpath, dataFieldTypes);
            string insertSql = SQLiteHelper.CreateInsertSql(dataFieldTypes);

            ExecuteFor(c =>
            {
                using (SQLiteConnection targetconn = new SQLiteConnection(sqliteConnString))
                {
                    targetconn.Open();
                    using (SQLiteCommand targetcmd = new SQLiteCommand(insertSql, targetconn))
                    {
                        targetcmd.CommandText = insertSql;
                        c.QueryRawDataReader(r =>
                        {
                            foreach (DataFieldType type in dataFieldTypes)
                            {
                                if (type.Type.ToLower() == "datetime")
                                {
                                    //日期数据为空 则默认为 1899-12-30
                                    string dateTime = DateTime.Parse("1899-12-30").ToString("s");
                                    if (!string.IsNullOrEmpty(r[type.FiledName].ToString()))
                                    {
                                        dateTime = Convert.ToDateTime(r[type.FiledName].ToString()).ToString("s");
                                    }
                                    targetcmd.Parameters.Add(new SQLiteParameter(type.FieldParameterName, dateTime));
                                }
                                else
                                {
                                    targetcmd.Parameters.Add(new SQLiteParameter(type.FieldParameterName, r[type.FiledName].ToString()));
                                }
                            }

                            targetcmd.ExecuteNonQuery();
                            targetcmd.Parameters.Clear();

                        }, sql, new { StartTime = op, EndTime = ed });

                        targetcmd.Dispose();
                    }
                    targetconn.Close();
                }
            }, connect_string);
            return true;
        }

        public DataTable GetDataList(string sql, DateTime op, DateTime ed)
        {
            try
            {
                if (string.IsNullOrEmpty(_default_connect_str))
                {
                    throw new Exception("数据库连接字符串未赋值");
                }

                using (IDbConnection con = new D() { ConnectionString = _default_connect_str })
                {
                    return con.QueryDT(sql, new { StartTime = op, EndTime = ed });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DBConnectOpen()
        {
            try
            {
                if (string.IsNullOrEmpty(_default_connect_str))
                {
                    throw new Exception("数据库连接字符串未赋值");
                }
                using (IDbConnection con = new D() { ConnectionString = _default_connect_str })
                {
                    con.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string DBConnectStr
        {
            get
            {
                return _default_connect_str;
            }
            set
            {
                _default_connect_str = value;
            }
        }

        /// <summary>
        /// 通过SQL语句获取所有列集合
        /// </summary>
        /// <param name="sql"></param>     
        /// <returns></returns>
        private DataFieldTypeCollection GetDataFieldCollection(string sql)
        {
            //1 获取表结构并不获取数据
            DataTable table = null;
            try
            {
                DateTime baseTime = DateTime.Now.AddYears(10).Date;
                table = GetDataList(sql, baseTime, baseTime);

                if (table == null || table.Columns.Count == 0)
                {
                    throw new Exception($"创建SQLite数据库时，SQL语句[{sql}]查询数据库成功，但未获取到数据表列字段");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"创建SQLite数据库时，通过SQL语句[{sql}]查询数据库出现错误,异常信息[{ex.Message}][{ex.StackTrace}]");
            }

            //2 获取表字段集合
            DataFieldTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(table);
            return dataTypeCollection;
        }
    }

    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters _dynamicParameters = new DynamicParameters();

        private readonly List<OracleParameter> _oracleParameters = new List<OracleParameter>();

        public void Add(string name, object value = null, DbType dbType = DbType.AnsiString, ParameterDirection? direction = null, int? size = null)
        {
            _dynamicParameters.Add(name, value, dbType, direction, size);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType) { Direction = direction };
            if (value != null)
            {
                oracleParameter.Value = value;
            }
            _oracleParameters.Add(oracleParameter);
        }

        public void AddArrayParamter(string name, object value, int size, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, size) { Direction = direction };

            oracleParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            if (value != null)
            {
                oracleParameter.Value = value;
            }
            _oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)_dynamicParameters).AddParameters(command, identity);

            var oracleCommand = command as OracleCommand;

            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(_oracleParameters.ToArray());
            }
        }

        public T Get<T>(string parameterName)
        {
            var parameter = _oracleParameters.SingleOrDefault(t => t.ParameterName == parameterName);
            if (parameter != null)
                return (T)Convert.ChangeType(parameter.Value.ToString(), typeof(T));
            return default(T);

        }

        public T Get<T>(int index)
        {
            var parameter = _oracleParameters[index];
            if (parameter != null)
                return (T)Convert.ChangeType(parameter.Value.ToString(), typeof(T));
            return default(T);
        }
    }


}


