using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.DAO;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Core.Service
{
   public class SystemConfigService
    {
        private SystemConfigDAO dao;
        public SystemConfigService()
        {
            dao = new SystemConfigDAO();
        }

        /// <summary>
        /// 查询数据库所有配置
        /// </summary>
        /// <returns></returns>
        public List<SystemConfigEntity> GetSystemConfigList()
        {
            return dao.GetSystemConfigList();
        }

        /// <summary>
        /// 获取一条配置
        /// </summary>
        /// <param name="sysConfigID"></param>
        /// <returns></returns>
        public SystemConfigEntity FindSystemConfig(int sysConfigID)
        {
            return dao.FindSystemConfig(sysConfigID);
        }

        /// <summary>
        /// 更新配置状态
        /// </summary>
        /// <param name="sysConfigID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateConfigStatus(int sysConfigID, int status)
        {
            return dao.UpdateConfigStatus(sysConfigID, status);
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="systemConfig"></param>
        /// <returns></returns>
        public bool Update(SystemConfigEntity systemConfig)
        {
            return dao.Update(systemConfig);
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="systemConfig"></param>
        /// <returns></returns>
        public bool Insert(SystemConfigEntity systemConfig)
        {
            return dao.Insert(systemConfig);
        }

    }
}
