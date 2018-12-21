using System;
using System.Collections.Generic;

namespace Transfer8Pro.Entity
{
    [Serializable]
    public class ClientConfigEntity
    {
        public List<TaskEntity> Tasks { get; set; }
        public string FtpAccount { get; set; }
        /// <summary>
        /// 密码需要加密
        /// </summary>
        public string FtpPwd_Hashed { get; set; }
        public string FtpIP { get; set; }

        public string UniqueID
        {
            get
            {
                return string.Format("{0}@{1}", FtpAccount, FtpIP);
            }
        }

        public string DataFolder { get; set; }
    }
}
