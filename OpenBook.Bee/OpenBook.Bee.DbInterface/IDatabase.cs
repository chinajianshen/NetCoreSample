using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Database
{
   public interface IDatabase
    {
        string ConnectionString { get; set; }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable ExcuteSqlForDataTable(string querySql, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 得到数据表结构
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable GetTableStructure(string querySql, DateTime startTime, DateTime endTime);


        /// <summary>
        /// 查询数据 并返回第一行第一列
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        object ExcuteScalarSql(string querySql, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获取 DataReader对象
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string querySql, DateTime startTime, DateTime endTime);


        /// <summary>
        /// 生成Access文件并添加数据
        /// </summary>
        /// <param name="accessFileFullpath"></param>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        void ExecuteDataToAccess(string accessFileFullpath, string querySql, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 创建SQLite文件并添加数据
        /// </summary>
        /// <param name="sqliteFileFullpath"></param>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        void ExecuteDataToSQLite(string sqliteFileFullpath, string querySql, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 生成数据库文件
        /// </summary>
        /// <param name="dbFileType"></param>
        /// <param name="dbFileFullpath"></param>
        /// <param name="querySql"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        void ExecuteDataToDBFile(DbFileType dbFileType, string dbFileFullpath, string querySql, DateTime startTime, DateTime endTime);

    }
}
