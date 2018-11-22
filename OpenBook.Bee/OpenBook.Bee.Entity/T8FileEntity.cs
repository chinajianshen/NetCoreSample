using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 数据文件类
    /// </summary>
    public class T8FileEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 生成文件信息
        /// </summary>
        public T8FileInfoEntity FileGenerateInfo { get; set; }

        /// <summary>
        /// 生成压缩文件信息
        /// </summary>

        public T8FileInfoEntity FileCompressedInfo { get; set; }

        /// <summary>
        /// 上传文件备份信息
        /// </summary>

        public T8FileInfoEntity FileUploadBackInfo { get; set; }

        /// <summary>
        /// 步骤状态
        /// </summary>
        public StepStatus StepStatus { get; set; }

        /// <summary>
        /// 日期类型
        /// </summary>
        public DateType DateType { get; set; }
    }

    /// <summary>
    /// 文件详细信息
    /// </summary>
    public class T8FileInfoEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件生成时间
        /// </summary>
        public DateTime FileGenerateTime { get; set; }
    }

    /// <summary>
    /// Ftp类
    /// </summary>
    public class FtpInfoEntity
    {
        /// <summary>
        /// FTP服务器地址 
        /// </summary>
        public string ServerAddress { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }
    }

    /// <summary>
    /// 数据库类
    /// </summary>
    public class DataBaseInfo
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        public string GetConnectionString
        {
            get
            {
                return "";
            }
        }

        public string UserName { get; set; }
    }
}
