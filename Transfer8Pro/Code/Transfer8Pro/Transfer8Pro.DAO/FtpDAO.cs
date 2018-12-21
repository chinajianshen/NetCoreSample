using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.DAO
{
    public class FtpDAO : DAOBase<SQLiteConnection>
    {
        public FtpDAO()
        {
            //string localDbConn = ConfigHelper.GetDBConnectStringConfig("LocalDBConnectStr");    
            base._default_connect_str = base.GetSqliteConnectString();
        }

        /// <summary>
        /// 新建或更新 
        /// </summary>
        /// <param name="ftpConfigEntity"></param>
        /// <returns></returns>
        public bool InsertOrUpdate(FtpConfigEntity ftpConfigEntity)
        {
            var sql = @"INSERT INTO T8_FtpConfig (ServerAddress,UserName,UserPassword,ExportFileDirectory,CreateTime,ServerDirectory) 
                       VALUES(@ServerAddress,@UserName,@UserPassword,@ExportFileDirectory,@CreateTime,@ServerDirectory)";
            var existSql = "SELECT COUNT(*) AS Cnt FROM T8_FtpConfig";
            var updateSql = @"UPDATE T8_FtpConfig SET ServerAddress=@ServerAddress,UserName=@UserName,UserPassword=@UserPassword,ExportFileDirectory=@ExportFileDirectory,ServerDirectory=@ServerDirectory";

            return base.ExecuteFor((conn) =>
            {
                var prms = new
                {
                    ServerAddress = ftpConfigEntity.ServerAddress,
                    UserName = ftpConfigEntity.UserName,
                    UserPassword = ftpConfigEntity.UserPassword,
                    ExportFileDirectory = ftpConfigEntity.ExportFileDirectory,
                    CreateTime = DateTime.Now.ToString("s"),
                    ServerDirectory =ftpConfigEntity.ServerDirectory
                };

                DataTable table = conn.QueryDT(existSql);
                if (table != null && table.Rows[0]["Cnt"].ToString().ToInt() > 0)
                {
                    return conn.Execute(updateSql, prms) > 0;
                }

                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ftpConfigEntity"></param>
        /// <returns></returns>
        public bool Update(FtpConfigEntity ftpConfigEntity)
        {
            var sql = @"UPDATE T8_FtpConfig SET ServerAddress=@ServerAddress,UserName=@UserName,UserPassword=@UserPassword,ExportFileDirectory=@ExportFileDirectory WHERE FtpID=@FtpID";
            return base.ExecuteFor((conn) =>
            {
                var prms = new
                {
                    FtpID = ftpConfigEntity.FtpID,
                    ServerAddress = ftpConfigEntity.ServerAddress,
                    UserName = ftpConfigEntity.UserName,
                    UserPassword = ftpConfigEntity.UserPassword,
                    ExportFileDirectory = ftpConfigEntity.ExportFileDirectory,
                    CreateTime = DateTime.Now.ToString("s")
                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <param name="ftpID"></param>
        /// <returns></returns>
        public bool Delete(int ftpID)
        {
            var sql = @"DELETE FROM T8_FtpConfig WHERE FtpID=@FtpID";
            return base.ExecuteFor(conn =>
            {
                var prms = new
                {
                    FtpID = ftpID
                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="ftpID"></param>
        /// <returns></returns>
        public FtpConfigEntity Find(int ftpID)
        {
            string sql = "SELECT * FROM T8_FtpConfig WHERE FtpID=@FtpID";
            return base.ExecuteFor(conn =>
            {
                var prms = new { FtpID = ftpID };
                return conn.Query<FtpConfigEntity>(sql, prms).FirstOrDefault();
            });
        }

        /// <summary>
        /// 获取首条数据
        /// </summary>      
        /// <returns></returns>
        public FtpConfigEntity GetFirstFtpInfo()
        {
            string sql = "SELECT * FROM T8_FtpConfig Limit 1";
            return base.ExecuteFor(conn =>
            {

                return conn.Query<FtpConfigEntity>(sql).FirstOrDefault();
            });
        }
    }
}
