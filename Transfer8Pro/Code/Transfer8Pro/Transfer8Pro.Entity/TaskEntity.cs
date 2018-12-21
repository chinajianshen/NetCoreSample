using System;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Entity
{
    [Serializable]
    public class TaskEntity
    {
        public TaskEntity()
        {
           
        }

        public string TaskID { get; set; }

        public DataTypes DataType { get; set; }

        public CycleTypes CycleType { get; set; }

        public string Cron { get; set; }

        public string DataHandler { get; set; }

        /// <summary>
        /// 数据库连接字符串需要加密存储
        /// </summary>
        public string DBConnectString_Hashed { get; set; }

        public string SQL { get; set; }

        public string TaskName { get; set; }      

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 启用状态 1启用 2已停用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus TaskStatus { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 最近执行时间
        /// </summary>
        public DateTime RecentRunTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireTime { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 文件详细信息
        /// </summary>
        public FileInfoEntity FileInfo { get; set; }

        /// <summary>
        /// Ftp配置信息
        /// </summary>
        public FtpConfigEntity FtpConfig { get; set; }

        /// <summary>
        ///任务唯一编码 用来确定任务是否有修改
        /// </summary>
        public string TaskUniqueCode
        {
            get
            {
                string taskString = $"{TaskID}{Cron}{DataHandler}{DBConnectString_Hashed}{SQL}{DataType}{CycleType}{Enabled}";
                return MD5Utils.GetMD5(taskString);
            }
        }
    }
}
