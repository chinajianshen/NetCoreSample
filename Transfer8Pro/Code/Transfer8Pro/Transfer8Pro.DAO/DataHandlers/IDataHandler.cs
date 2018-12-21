using System;
using System.Data;

namespace Transfer8Pro.DAO.DataHandlers
{
    public interface IDataHandler
    {
        bool OutputData(string sql, DateTime op, DateTime ed, string target_jl_file, string connect_string = null);

        bool OutputDataToSQLite(string sql, DateTime op, DateTime ed, string sqliteFileFullpath, string connect_string = null);

         string DBConnectStr { get; set; }


        /// <summary>
        /// 连接数据库
        /// </summary>   
        /// <returns></returns>
        bool DBConnectOpen();

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="op"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        DataTable GetDataList(string sql, DateTime op, DateTime ed);

    }

    public class DBTypeNameAttribute : Attribute
    {
        public DBTypeNameAttribute()
        { }

        public DBTypeNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
