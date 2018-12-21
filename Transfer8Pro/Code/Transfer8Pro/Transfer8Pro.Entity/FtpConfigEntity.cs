using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// Ftp类
    /// </summary>   
    [Serializable]
    public class FtpConfigEntity
    {
        public int FtpID { get; set; }
        /// <summary>
        /// FTP服务器地址 
        /// </summary>
        public string ServerAddress { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        /// <summary>
        /// 导出文件目录
        /// </summary>
        public string ExportFileDirectory { get; set; }

        /// <summary>
        /// FTP服务器文件目录
        /// </summary>
        public string ServerDirectory { get; set; }
    }
}
