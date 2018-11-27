using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.DbInterface
{
   public interface IDatabase
    {
        IDbConnection Connection { get; }
        string ConnectionString { get; set; }

        void Open();
        void Close();
        void Dispose();


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable ExcuteSqlForDataTable(string querySql, string startTime, string endTime);

        /// <summary>
        /// 得到数据表结构
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable GetTableStructure(string querySql, string startTime, string endTime);


        /// <summary>
        /// 查询数据 并返回第一行第一列
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        object ExcuteScalarSql(string querySql, string startTime, string endTime);

        /// <summary>
        /// 获取 DataReader对象
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string querySql, string startTime, string endTime);


        /// <summary>
        /// 生成Access文件
        /// </summary>
        /// <param name="dbFileFullPath"></param>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        void ExecuteDataToAccess(string dbFileFullPath, string querySql, string startTime, string endTime);

        /// <summary>
        /// 生成SQLite文件
        /// </summary>
        /// <param name="dbFileFullPath"></param>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        void ExecuteDataToSQLite(string dbFileFullPath, string querySql, string startTime, string endTime);

    }
}
