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
    [Serializable]
    public class T8FileEntity
    {       

        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlString { get; set; }

        /// <summary>
        /// SQL查询开始时间
        /// </summary>
        public DateTime SqlStartTime { get; set; }

        /// <summary>
        /// Sql查询结束时间
        /// </summary>
        public DateTime SqlEndTime { get; set; }

        /// <summary>
        /// 数据文件类型
        /// </summary>
        public DbFileType DbFileType { get; set; }

        /// <summary>
        /// 步骤状态 (每步完成需要设置状态)
        /// </summary>
        public StepStatus StepStatus { get; set; }

        /// <summary>
        /// 日期类型
        /// </summary>
        public DateType DateType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DataType { get; set; }

        // 任务源类型
        /// </summary>
        public TaskSourceType TaskSourceType { get; set; }

        /// <summary>
        /// Ftp类
        /// </summary>
        public FtpInfoEntity FtpInfo { get; set; }

        /// <summary>
        /// 数据库信息
        /// </summary>
        public DataBaseInfoEntity DataBaseInfo { get; set; }

        /// <summary>
        /// 生成文件信息
        /// </summary>
        public T8FileInfoEntity GeneralFileInfo { get; set; }

        /// <summary>
        /// 生成压缩文件信息
        /// </summary>

        public T8FileInfoEntity CompressFileInfo { get; set; }        

        /// <summary>
        /// 上传文件备份信息
        /// </summary>
        public T8FileInfoEntity UploadBackFileInfo { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Quartz Cron表达式
        /// </summary>
        public string QuartzCronExpression { get; set; }
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
}
