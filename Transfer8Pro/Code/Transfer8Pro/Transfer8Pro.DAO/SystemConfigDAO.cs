using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Dapper;
using Transfer8Pro.Entity;

namespace Transfer8Pro.DAO
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfigDAO : DAOBase<SQLiteConnection>
    {
        public SystemConfigDAO()
        {
            base._default_connect_str = base.GetSqliteConnectString();
        }

        /// <summary>
        /// 查询数据库所有配置
        /// </summary>
        /// <returns></returns>
        public List<SystemConfigEntity> GetSystemConfigList()
        {
            string sql = "SELECT * FROM T8_SystemConfig";
            return base.ExecuteFor(conn =>
            {
                return conn.Query<SystemConfigEntity>(sql).ToList();
            });
        }

        /// <summary>
        /// 获取一条配置
        /// </summary>
        /// <param name="sysConfigID"></param>
        /// <returns></returns>
        public SystemConfigEntity FindSystemConfig(int sysConfigID)
        {
            string sql = "SELECT * FROM T8_SystemConfig WHERE SysConfigID=@SysConfigID";
            return base.ExecuteFor(conn =>
            {
                return conn.Query<SystemConfigEntity>(sql, new { SysConfigID = sysConfigID }).FirstOrDefault();
            });
        }

        /// <summary>
        /// 更新配置状态
        /// </summary>
        /// <param name="sysConfigID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateConfigStatus(int sysConfigID, int status)
        {
            string sql = "UPDATE T8_SystemConfig SET Status=@Status WHERE SysConfigID=@SysConfigID";
            return base.ExecuteFor(conn =>
            {
                return conn.Execute(sql, new { Status = status, SysConfigID = sysConfigID }) > 0;
            });
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="systemConfig"></param>
        /// <returns></returns>
        public bool Update(SystemConfigEntity systemConfig)
        {
            string sql = "UPDATE T8_SystemConfig SET SysConfigName=@SysConfigName,Status=@Status,Cron=@Cron,ExSetting01=@ExSetting01,ExSetting02=@ExSetting02 WHERE SysConfigID=@SysConfigID";

            return base.ExecuteFor(conn =>
            {
                var prms = new
                {
                    SysConfigName = systemConfig.SysConfigName,
                    SysConfigID = systemConfig.SysConfigID,
                    Status = systemConfig.Status,
                    Cron = systemConfig.Cron,
                    ExSetting01 = systemConfig.ExSetting01,
                    ExSetting02 = systemConfig.ExSetting02
                };

                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="systemConfig"></param>
        /// <returns></returns>
        public bool Insert(SystemConfigEntity systemConfig)
        {
            string sql = "INSERT INTO T8_SystemConfig(SysConfigID,SysConfigName,Status,Cron,ExSetting01,ExSetting02)VALUES(@SysConfigID,@SysConfigName,@Status,@Cron,@ExSetting01,@ExSetting02)";

            return base.ExecuteFor(conn =>
            {
                var prms = new
                {
                    SysConfigName = systemConfig.SysConfigName,
                    SysConfigID = systemConfig.SysConfigID,
                    Status = systemConfig.Status,
                    Cron = systemConfig.Cron,
                    ExSetting01 = systemConfig.ExSetting01,
                    ExSetting02 = systemConfig.ExSetting02
                };

                return conn.Execute(sql, prms) > 0;
            });
        }
    }
}
