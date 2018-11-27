using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utility
{
    public class DataBaseHelper
    {
        #region Connection string

        public static string CreateConnectionString(DataBaseInfoEntity dataBaseInfo)
        {
            return CreateConnectionString(dataBaseInfo.DataBaseType, dataBaseInfo.ServerName, dataBaseInfo.ServerName, dataBaseInfo.UserName, dataBaseInfo.UserPassword);
        }

        public static string CreateConnectionString(DatabaseType dbType, string dbHost, string dbName, string dbUser, string dbPwd)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return CreateSQLServerConnectionString(dbHost, dbName, dbUser, dbPwd);

                case DatabaseType.MySQL:
                    return CreateMySQLConnectionString(dbHost, dbName, dbUser, dbPwd);

                case DatabaseType.Oracle:
                    return CreateOracleConnectionString(dbHost, dbName, dbUser, dbPwd);

                default:
                    throw new Exception("Unsupported database type.");
            }
        }

        public static string CreateSQLServerConnectionString(string dbHost, string dbName, string dbUser, string dbPwd)
        {
            //优先从配置中读取 后期添加

            var sb = new StringBuilder();
            sb.AppendFormat("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", dbHost, dbName, dbUser, dbPwd);
            return sb.ToString();
        }

        public static string CreateMySQLConnectionString(string dbHost, string dbName, string dbUser, string dbPwd)
        {
            //优先从配置中读取 后期添加

            var sb = new StringBuilder();
            sb.AppendFormat("server={0};database={1};uid={2};pwd={3};", dbHost, dbName, dbUser, dbPwd);
            return sb.ToString();
        }

        public static string CreateOracleConnectionString(string dbHost, string dbName, string dbUser, string dbPwd)
        {
            //优先从配置中读取 后期添加

            var sb = new StringBuilder();
            sb.AppendFormat("password={0};User ID={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={3})))",
                dbPwd, dbUser, dbHost, dbName);
            return sb.ToString();
        }

        public static string CreateAccessFileConnectionString(string accessFileFullpath)
        {
            //优先从配置中读取 后期添加

            return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={accessFileFullpath}";
        }

        public static string CreateSQLiteFileConnnectionString(string sqliteFileFullpath)
        {
            //优先从配置中读取 后期添加

            return $"Data Source={sqliteFileFullpath};Version=3;";
        }

        #endregion

        #region DataSet helper

        public static int GetRowCount(DataSet dataset)
        {
            return dataset.Tables[0].Rows.Count;
        }

        public static int GetColumnCount(DataSet dataset)
        {
            return dataset.Tables[0].Columns.Count;
        }

        public static object GetValue(DataSet dataset, int row, string columnName)
        {
#if DEBUG
            if (row >= GetRowCount(dataset))
                throw new IndexOutOfRangeException(string.Format("Row out of bounds when get value from db dataset for {0} - row {1}", row, columnName));
#endif
            return dataset.Tables[0].Rows[row][columnName];
        }

        public static int GetIntValue(DataSet dataset, int row, int columnIndex)
        {
#if DEBUG
            if (row >= GetRowCount(dataset))
                throw new IndexOutOfRangeException(string.Format("Row out of bounds when get int value from db dataset for {0} - {1}", row, columnIndex));
            if (columnIndex >= GetColumnCount(dataset))
                throw new IndexOutOfRangeException(string.Format("Column out of bounds when get int value from db dataset for {0} - {1}", row, columnIndex));
#endif

            return Convert.ToInt32(dataset.Tables[0].Rows[row][columnIndex]);
        }

        public static object GetValue(DataSet dataset, int row, int columnIndex)
        {
#if DEBUG
            if (row >= GetRowCount(dataset))
                throw new IndexOutOfRangeException(string.Format("Row out of bounds when get value from db dataset for {0} - {1}", row, columnIndex));
            if (columnIndex >= GetColumnCount(dataset))
                throw new IndexOutOfRangeException(string.Format("Column out of bounds when get value from db dataset for {0} - {1}", row, columnIndex));
#endif
            return dataset.Tables[0].Rows[row][columnIndex];
        }

        #endregion

        #region Validate SQL command

        public static string ValidateSQLCommand(DatabaseType dbType, string sqlCmd)
        {
            if (string.IsNullOrEmpty(sqlCmd))
                return sqlCmd;

            var retVal = new StringBuilder();
            if (dbType == DatabaseType.SqlServer)
            {
                foreach (char t in sqlCmd)
                {
                    switch (t)
                    {
                        case '\'':
                            retVal.Append("''");
                            break;
                        case '"':
                            retVal.Append("\"\"");
                            break;
                        default:
                            retVal.Append(t);
                            break;
                    }
                }
            }
            else if (dbType == DatabaseType.MySQL)
            {
                foreach (char t in sqlCmd)
                {
                    switch (t)
                    {
                        case '\'':
                            retVal.Append("\\'");
                            break;
                        case '\\':
                            retVal.Append("\\\\");
                            break;
                        default:
                            retVal.Append(t);
                            break;
                    }
                }
            }
            else
            {
                // TODO:Oracle
                retVal.Append(sqlCmd);
            }

            return retVal.ToString();
        }

        #endregion

        /// <summary>
        /// 过滤SQL语句 只留下 如 select * from a
        /// </summary>
        /// <param name="querySql"></param>
        public static string FilterSqlQuery(string querySql)
        {
            Regex sqlFilterReg = new Regex(@".*?(?<Sql>select\s*.*?)order? .*?", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match math = sqlFilterReg.Match(querySql);

            string sql = "";
            if (math.Success)
            {
                sql = $"select * from ({math.Groups["Sql"].ToString()}) tab where 1<>1";
            }
            else
            {
                throw new Exception("SQL正则过滤失败");
            }
            return sql;
        }

        /// <summary>
        /// 创建参数列表
        /// </summary>
        /// <param name="dataFieldTypes"></param>
        /// <returns></returns>
        public static List<string> CreateParameterNameList(DataFieldTypeCollection dataFieldTypes)
        {
           return  dataFieldTypes.Select(item => "@" + item.FiledName).ToList();           
        }

        /// <summary>
        /// 构造Access和Sqlite数据文件 SQL插入语句
        /// </summary>
        /// <param name="dataFieldTypes"></param>
        /// <returns></returns>
        public static string CreateInsertSql(DataFieldTypeCollection dataFieldTypes)
        {
            string fieldString = string.Join(",", dataFieldTypes.Select(item => item.FiledName));
            string parameterNameString = string.Join(",", dataFieldTypes.Select(item => "@" + item.FiledName));
            string insertSql = $"insert into [mytable]({fieldString}) values({parameterNameString})";
            return insertSql;
        }

        /// <summary>
        /// 转换成可执行SQL语句
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string CovertedExecuteSql(string querySql, string startTime, string endTime)
        {
            return querySql.Replace("@StartTime", $"'{startTime}'").Replace("@EndTime", $"'{endTime}'");
        }

    }
}
