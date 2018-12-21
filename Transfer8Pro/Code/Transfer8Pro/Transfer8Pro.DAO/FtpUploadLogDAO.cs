using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;
using Dapper;

namespace Transfer8Pro.DAO
{
    /// <summary>
    /// FTP上传日志
    /// </summary>
    public class FtpUploadLogDAO : DAOBase<SQLiteConnection>
    {
        public FtpUploadLogDAO()
        {
            base._default_connect_str = base.GetSqliteConnectString();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="uploadLogEntity"></param>
        /// <returns></returns>
        public bool Insert (FtpUploadLogEntity uploadLogEntity)
        {
            string sql = "";

            return base.ExecuteFor(conn =>
            {
                var prms = new
                {

                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="uploadLogEntity"></param>
        /// <returns></returns>
        public bool Update(FtpUploadLogEntity uploadLogEntity)
        {
            string sql = "";

            return base.ExecuteFor(conn =>
            {
                var prms = new
                {

                };
                return conn.Execute(sql, prms) > 0;
            });
        }
    }
}
