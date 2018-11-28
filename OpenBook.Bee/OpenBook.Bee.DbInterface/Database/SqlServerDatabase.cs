using OpenBook.Bee.Database.Database.SqlHelper;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Database.Utility;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace OpenBook.Bee.Database.Database
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

        public object ExcuteScalarSql(string querySql, DateTime startTime, DateTime endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteScalar(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms);
        }

        public DataTable ExcuteSqlForDataTable(string querySql, DateTime startTime, DateTime endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteDataset(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms).Tables[0];
        }

        public IDataReader ExecuteReader(string querySql, DateTime startTime, DateTime endTime)
        {
            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteReader(SQlServerHelper.MyConnectStr, CommandType.Text, querySql, prms);
        }

        public DataTable GetTableStructure(string querySql, DateTime startTime, DateTime endTime)
        {
            string sql = DataBaseHelper.FilterSqlQuery(querySql);

            SqlParameter[] prms = this.ConvetSqlParameter(startTime, endTime);
            return SQlServerHelper.ExecuteDataset(SQlServerHelper.MyConnectStr, CommandType.Text, sql, prms).Tables[0];
        }

        public void ExecuteDataToDBFile(DbFileType dbFileType, string dbFileFullpath, string querySql, DateTime startTime, DateTime endTime)
        {
            switch (dbFileType)
            {
                case DbFileType.SQLite:
                    this.ExecuteDataToSQLite(dbFileFullpath, querySql, startTime, endTime);
                    break;
                case DbFileType.Access:
                    this.ExecuteDataToAccess(dbFileFullpath, querySql, startTime, endTime);
                    break;
                default:
                    throw new ArgumentNullException("DbFileType未赋值");                   
            }
        }


        public void ExecuteDataToAccess(string accessFileFullpath, string querySql, DateTime startTime, DateTime endTime)
        {
            //获取数据表构架
            DataTable dt = this.GetTableStructure(querySql, startTime, endTime);
            if (dt.Columns.Count == 0)
            {
                throw new Exception($"执行语句[{DataBaseHelper.CovertedExecuteSql(querySql, startTime, endTime)}]查询，数据表架构列为空");
            }
            DataFieldTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(dt);
            string accessConnString = DataBaseHelper.CreateAccessFileConnectionString(accessFileFullpath);

            //Insert语句
            string insertSql = DataBaseHelper.CreateInsertSql(dataTypeCollection);

            //创建SQLite数据库
            AccessHelper.CreateDataBase(accessConnString, accessFileFullpath, dataTypeCollection);

            using (OleDbConnection targetconn = new OleDbConnection(accessConnString))
            {
                targetconn.Open();
                using (IDbCommand targetcmd = new OleDbCommand(insertSql, targetconn))
                {
                    //targetcmd.CommandText = insertSql;

                    #region 数据库操作部分
                    using (IDbConnection sourceconn = new SqlConnection(SQlServerHelper.MyConnectStr))
                    {
                        sourceconn.Open();
                        using (IDataReader sourceReader = this.ExecuteReader(querySql, startTime, endTime))
                        {
                            while (sourceReader.Read())
                            {
                                foreach (DataFieldType type in dataTypeCollection)
                                {
                                    int len = sourceReader[type.FiledName].ToString().Length;
                                    if (len > 255)
                                    {
                                        OleDbParameter oldb = new OleDbParameter(type.FieldParameterName, OleDbType.VarChar, len + 5);
                                        oldb.Value = sourceReader[type.FiledName].ToString();
                                        targetcmd.Parameters.Add(oldb);
                                    }
                                    else
                                    {
                                        targetcmd.Parameters.Add(new OleDbParameter(type.FieldParameterName, sourceReader[type.FiledName].ToString()));
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

                targetconn.Close();
            }
        }

        public void ExecuteDataToSQLite(string sqliteFileFullpath, string querySql, DateTime startTime, DateTime endTime)
        {
            //获取数据表构架
            DataTable dt = this.GetTableStructure(querySql, startTime, endTime);
            if (dt.Columns.Count == 0)
            {
                throw new Exception($"执行语句[{DataBaseHelper.CovertedExecuteSql(querySql, startTime, endTime)}]查询，数据表架构列为空");
            }
            DataFieldTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(dt);
            string sqliteConnString = DataBaseHelper.CreateSQLiteFileConnectionString(sqliteFileFullpath);

            //Insert语句
            string insertSql = DataBaseHelper.CreateInsertSql(dataTypeCollection);

            //创建SQLite数据库
            SQLiteHelper.CreateDataBase(sqliteConnString, sqliteFileFullpath, dataTypeCollection);

            using (SQLiteConnection targetconn = new SQLiteConnection(sqliteConnString))
            {
                targetconn.Open();
                using (SQLiteCommand targetcmd = new SQLiteCommand(insertSql, targetconn))
                {
                    targetcmd.CommandText = insertSql;

                    #region 数据库操作部分
                    using (IDbConnection sourceconn = new SqlConnection(SQlServerHelper.MyConnectStr))
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
                                        targetcmd.Parameters.Add(new SQLiteParameter(type.FieldParameterName, dateTime));
                                    }
                                    else
                                    {
                                        targetcmd.Parameters.Add(new SQLiteParameter(type.FieldParameterName, sourceReader[type.FiledName].ToString()));
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

                targetconn.Close();
            }
        }


        private SqlParameter[] ConvetSqlParameter(DateTime startTime, DateTime endTime)
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
