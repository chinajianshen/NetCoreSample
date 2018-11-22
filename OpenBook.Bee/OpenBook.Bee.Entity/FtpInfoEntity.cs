using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// Ftp类
    /// </summary>
    [Serializable]
    public class FtpInfoEntity
    {
        /// <summary>
        /// FTP服务器地址 
        /// </summary>
        public string ServerAddress { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }
    }
}
