using OpenBook.Bee.Database.Database.SqlHelper;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utility;
using OpenBook.Bee.Utils;
using OpenBook.BeeDatabase.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenBook.Bee.Database
{
    public class SqlServerDatabase : IDatabase
    {
        private string _connectionString;

        public SqlServerDatabase()
        {
        }

        public SqlServerDatabase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("ConnectionString is null!");

            SQlServerHelper.MyConnectStr = connectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("ConnectionString is null!");

                _connectionString = value;
                SQlServerHelper.MyConnectStr = _connectionString;
            }
        }

        public object ExcuteScalarSql(string querySql, string startTime, string endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteScalar(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms);
        }

        public DataTable ExcuteSqlForDataTable(string querySql, string startTime, string endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteDataset(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms).Tables[0];
        }

        public IDataReader ExecuteReader(string querySql, string startTime, string endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteReader(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms);
        }

        public DataTable GetTableStructure(string querySql, string startTime, string endTime)
        {
            string sql = DataBaseHelper.FilterSqlQuery(querySql);

            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteDataset(SQlServerHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
        }


        public void ExecuteDataToAccess(string accessFileFullpath, string querySql, string startTime, string endTime)
        {
            //获取数据表构架
            DataTable dt = this.GetTableStructure(querySql, startTime, endTime);
            if (dt.Columns.Count == 0)
            {
                throw new Exception($"执行语句[{DataBaseHelper.CovertedExecuteSql(querySql, startTime, endTime)}]查询，数据表架构列为空");
            }
            DataFieldTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(dt);
            string sqliteConnString = DataBaseHelper.CreateSQLiteFileConnnectionString(accessFileFullpath);
            IList<string> prms = DataBaseHelper.CreateParameterNameList(dataTypeCollection);

            //Insert语句
            string insertSql = DataBaseHelper.CreateInsertSql(dataTypeCollection);

            //创建SQLite数据库
            SQLiteHelper.CreateDataBase(sqliteConnString, accessFileFullpath, dataTypeCollection);

            using (SQLiteConnection targetconn = new SQLiteConnection(sqliteConnString))
            {
                targetconn.Open();
                using (IDbCommand targetcmd = new SQLiteCommand(targetconn))
                {
                    targetcmd.CommandText = insertSql;

                    #region 数据库操作部分
                    using (SqlConnection sourceconn = new SqlConnection(SQlServerHelper.MyConnectStr))
                    {
                        sourceconn.Open();
                        using (IDataReader sourceReader = this.ExecuteReader(querySql, startTime, endTime))
                        {
                            while (sourceReader.Read())
                            {
                                foreach (DataFieldType type in dataTypeCollection)
                                {
                                    if (type.Type.ToLower() == "datetime")
                                    {
                                        //解决SQLite查询时,日期数据报错
                                        string dateTime = "1899/12/30";
                                        if (!string.IsNullOrEmpty(sourceReader[type.FiledName].ToString()))
                                        {
                                            dateTime = Convert.ToDateTime(sourceReader[type.FiledName].ToString()).ToString("s");
                                        }
                                        targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, dateTime));
                                    }
                                    else
                                    {
                                        targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, sourceReader[type.FiledName].ToString()));
                                    }
                                }

                                targetcmd.ExecuteNonQuery();
                                targetcmd.Parameters.Clear();
                            }

                            sourceReader.Close();
                        }

                        sourceconn.Close();
                    }
                    #endregion

                    targetcmd.Dispose();
                }

                targetconn.Clone();
            }
        }

        public void ExecuteDataToSQLite(string sqliteFileFullpath, string querySql, string startTime, string endTime)
        {
            //获取数据表构架
            DataTable dt = this.GetTableStructure(querySql, startTime, endTime);
            if (dt.Columns.Count == 0)
            {
                throw new Exception($"执行语句[{DataBaseHelper.CovertedExecuteSql(querySql, startTime, endTime)}]查询，数据表架构列为空");
            }
            DataFieldTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(dt);
            string sqliteConnString = DataBaseHelper.CreateSQLiteFileConnnectionString(sqliteFileFullpath);
            IList<string> prms = DataBaseHelper.CreateParameterNameList(dataTypeCollection);

            //Insert语句
            string insertSql = DataBaseHelper.CreateInsertSql(dataTypeCollection);

            //创建SQLite数据库
            SQLiteHelper.CreateDataBase(sqliteConnString, sqliteFileFullpath, dataTypeCollection);

            using (SQLiteConnection targetconn = new SQLiteConnection(sqliteConnString))
            {
                targetconn.Open();
                using (IDbCommand targetcmd = new SQLiteCommand(targetconn))
                {
                    targetcmd.CommandText = insertSql;

                    #region 数据库操作部分
                    using (SqlConnection sourceconn = new SqlConnection(SQlServerHelper.MyConnectStr))
                    {
                        sourceconn.Open();
                        using (IDataReader sourceReader = this.ExecuteReader(querySql, startTime, endTime))
                        {
                            while (sourceReader.Read())
                            {
                                foreach (DataFieldType type in dataTypeCollection)
                                {
                                    if (type.Type.ToLower() == "datetime")
                                    {
                                        //解决SQLite查询时,日期数据报错
                                        string dateTime = "1899/12/30";
                                        if (!string.IsNullOrEmpty(sourceReader[type.FiledName].ToString()))
                                        {
                                            dateTime = Convert.ToDateTime(sourceReader[type.FiledName].ToString()).ToString("s");
                                        }
                                        targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, dateTime));
                                    }
                                    else
                                    {
                                        targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, sourceReader[type.FiledName].ToString()));
                                    }
                                }

                                targetcmd.ExecuteNonQuery();
                                targetcmd.Parameters.Clear();
                            }

                            sourceReader.Close();
                        }

                        sourceconn.Close();
                    }
                    #endregion

                    targetcmd.Dispose();
                }

                targetconn.Clone();
            }            
        }


        private SqlParameter[] ConvetSqlParameter(string startTime, string endTime)
        {
            SqlParameter[] prms =
            {
                new SqlParameter("@StartTime",startTime),
                new SqlParameter("@EndTime",endTime)
            };
            return prms;
        }
    }
}
