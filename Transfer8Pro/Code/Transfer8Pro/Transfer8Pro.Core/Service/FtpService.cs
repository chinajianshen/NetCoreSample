using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.DAO;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Core.Service
{
    public class FtpService
    {
        FtpDAO dao = null;
        public FtpService()
        {
            dao = new FtpDAO();
        }

        /// <summary>
        /// 新建或更新
        /// </summary>
        /// <param name="ftpConfigEntity"></param>
        /// <returns></returns>
        public bool InsertOrUpdate(FtpConfigEntity ftpConfigEntity)
        {
            if (!string.IsNullOrEmpty(ftpConfigEntity.UserPassword))
            {
                ftpConfigEntity.UserPassword = Common.EncryptData(ftpConfigEntity.UserPassword);
            }
            return dao.InsertOrUpdate(ftpConfigEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ftpConfigEntity"></param>
        /// <returns></returns>
        public bool Update(FtpConfigEntity ftpConfigEntity)
        {
            return dao.Update(ftpConfigEntity);
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <param name="ftpID"></param>
        /// <returns></returns>
        public bool Delete(int ftpID)
        {
            return dao.Delete(ftpID);
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="ftpID"></param>
        /// <returns></returns>
        public FtpConfigEntity Find(int ftpID)
        { 
            FtpConfigEntity ftpConfigEntity = dao.Find(ftpID);
            if (ftpConfigEntity != null)
            {
                ftpConfigEntity.UserPassword = Common.DecryptData(ftpConfigEntity.UserPassword);
            }
            return ftpConfigEntity;
        }

        /// <summary>
        /// 获取首条数据
        /// </summary>    
        /// <returns></returns>
        public FtpConfigEntity GetFirstFtpInfo()
        {
            FtpConfigEntity ftpConfigEntity = dao.GetFirstFtpInfo();
            if (ftpConfigEntity != null)
            {
                ftpConfigEntity.UserPassword = Common.DecryptData(ftpConfigEntity.UserPassword);
            }
            return ftpConfigEntity;
        }
    }
}
