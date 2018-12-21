using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// FTP上传记录
    /// </summary>
    public class FtpUploadLogEntity
    {
        public int FtpUploadID { get; set; }

        public string TaskID { get; set; }

        public string FileFullPath { get; set; }

        public string FileName { get; set; }

        public DateTime UploadStartTime { get; set; }

        public DateTime UploadEndTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string ElapsedTime { get; set; }

        /// <summary>
        /// 上传状态
        /// </summary>
        public FtpUploadStatus FtpUploadStatus { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }
    }
}
