using System.Collections.Generic;
using System.Data;

namespace DapperInCoreLib
{
    public interface IDapper
    {
        #region SELECT
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        List<T> GetList<T>(string sqlString);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<T> GetList<T>(string sqlString, object param);
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
        bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);

        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="list"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        #endregion

        #region UPDATE
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        #endregion

        #region DELETE
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeOut"></param>
        /// <returns></returns>
        bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        #endregion
    }
}