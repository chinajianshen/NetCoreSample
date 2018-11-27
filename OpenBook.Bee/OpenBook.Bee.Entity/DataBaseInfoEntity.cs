using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 数据库类
    /// </summary>
    [Serializable]
    public class DataBaseInfoEntity
    {
        /// <summary>
        /// 数据库汉语名称
        /// </summary>
        public string DataBaseTitle { get; set; }       

        /// <summary>
        /// 服务器名或服务名
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DataBaseType { get; set; }
    }
}
