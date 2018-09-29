using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DapperInCoreDemo
{
    /// <summary>
    /// 简单封装Dapper操作
    /// </summary>
    public static class DapperExtension
    {
        #region 连接字符串
        private static string CONN_STRING = "server=.;uid=sa;pwd=sa.;database=student";
        #endregion

        #region SELECT
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(this string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            var list = new List<T>();

            using (var db = new SqlConnection(CONN_STRING))
            {
                IEnumerable<T> ts = null;
                if (null == param)
                {
                    ts = db.Query<T>(sqlString, null, null, true, commandTimeout, commandType);
                }
                else
                {
                    ts = db.Query<T>(sqlString, param, null, true, commandTimeout, commandType);
                }
                if (null != ts)
                {
                    list = ts.AsList();
                }
            }

            return list;
        }

        public static List<T> GetList<T>(this string sqlString)
        {
            return GetList<T>(sqlString, null, CommandType.Text);
        }

        public static List<T> GetList<T>(this string sqlString, object param)
        {
            if (null == param)
            {
                return GetList<T>(sqlString);
            }

            return GetList<T>(sqlString, param);
        }
        #endregion

        #region INSERT
        /// <summary>
        /// 单条数据写入 动态模板模式/T
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        public static bool Insert(this string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="list"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        public static bool Insert<T>(this string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;

            if (null != list && 0 < list.Count)
            {
                using (var db = new SqlConnection(CONN_STRING))
                {
                    intResult = db.Execute(sqlString, list, null, commandTimeOut, commandType);                    
                }
            }

            return intResult > 0;
        }
        #endregion

        #region UPDATE
        public static bool Update(this string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region DELETE
        public static bool Delete(this string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region Private Methods
        private static bool ExecuteNonQuery(this string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;
            using (var db = new SqlConnection(CONN_STRING))
            {
                if (null == param)
                {
                    intResult = db.Execute(sqlString, null, null, commandTimeOut, commandType);

                }
                else
                {
                    intResult = db.Execute(sqlString, param, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion
    }
}