using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    /// <summary>
    /// 任务文件处理
    /// </summary>
   public class TaskFileProcess
    {
        
    }

    #region 生成文件名
    /// <summary>
    /// 生成文件名策略
    /// </summary>
    public class GenerateFileNameStragety
    {
        private AFileName _AFileName;
        public GenerateFileNameStragety(AFileName aFileName)
        {
            _AFileName = aFileName;
        }

        /// <summary>
        /// 文件名
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <returns></returns>
        public string FileName(T8FileEntity t8FileEntity)
        {
            return _AFileName.FileName(t8FileEntity);
        }

        /// <summary>
        /// 文件完全路径
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <returns></returns>
        public  string FileFullName(T8FileEntity t8FileEntity)
        {
            return _AFileName.FileFullName(t8FileEntity);
        }
    }

    /// <summary>
    /// 生成文件名及路径
    /// </summary>
    public abstract class AFileName
    {
        protected readonly string filePath;
        protected string fileName;

        public AFileName()
        {
            filePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 文件名
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <returns></returns>
        public abstract string FileName(T8FileEntity t8FileEntity);

        /// <summary>
        /// 文件完全路径
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <returns></returns>
        public abstract string FileFullName(T8FileEntity t8FileEntity);

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <returns></returns>
        protected virtual string GetFileName(T8FileEntity t8FileEntity)
        {
            if (string.IsNullOrEmpty(this.fileName))
            {
                string suffix = Common.GetDateTypeName(t8FileEntity.DateType) + Common.GetDataTypeName(t8FileEntity.DataType);
                string extName = Common.GetDBFileExtName(t8FileEntity.DbFileType);
                fileName = $"{t8FileEntity.FtpInfo.UserName}_{t8FileEntity.SqlStartTime.ToString("yyyyMMdd")}_{t8FileEntity.SqlEndTime.ToString("yyyyMMdd")}_{suffix}{extName}";
            }
            return fileName;
        }


        /// <summary>
        /// 文件完全路径
        /// </summary>
        /// <param name="t8FileEntity"></param>
        /// <param name="type">1一般文件目录 2压缩文件目录 3上传备份文件目录</param>
        /// <returns></returns>
        public string GetFileFullName(T8FileEntity t8FileEntity,int type)
        {
            string dbFilePath = Path.Combine(filePath, Common.GetDirName(t8FileEntity.TaskSourceType));
            dbFilePath = Path.Combine(dbFilePath, Common.GetSubDirName(type));

            if (string.IsNullOrEmpty(this.fileName))
            {
                string suffix = Common.GetDateTypeName(t8FileEntity.DateType) + Common.GetDataTypeName(t8FileEntity.DataType);
                string extName = Common.GetDBFileExtName(t8FileEntity.DbFileType);
                fileName = $"{t8FileEntity.FtpInfo.UserName}_{t8FileEntity.SqlStartTime.ToString("yyyyMMdd")}_{t8FileEntity.SqlEndTime.ToString("yyyyMMdd")}_{suffix}{extName}";
            }

            dbFilePath = Path.Combine(dbFilePath, fileName);
            return dbFilePath;
        }
    }

    /// <summary>
    /// 一般文件名称
    /// </summary>
    public class GeneralFileName : AFileName
    {
        public override string FileFullName(T8FileEntity t8FileEntity)
        {
            return base.GetFileFullName(t8FileEntity, 1);
        }

        public override string FileName(T8FileEntity t8FileEntity)
        {
            return base.GetFileName(t8FileEntity);
        }
    }

    /// <summary>
    /// 压缩文件名
    /// </summary>
    public class CompressFileName : AFileName
    {
        public override string FileFullName(T8FileEntity t8FileEntity)
        {
            return base.GetFileFullName(t8FileEntity,2);
        }

        public override string FileName(T8FileEntity t8FileEntity)
        {
            return base.GetFileName(t8FileEntity);
        }
    }

    /// <summary>
    /// 上传文件备份文件名
    /// </summary>
    public class UploadBackFileName : AFileName
    {
        public override string FileFullName(T8FileEntity t8FileEntity)
        {
            if (t8FileEntity.CompressFileInfo == null || string.IsNullOrEmpty(t8FileEntity.CompressFileInfo.FileName))
            {                
                throw new Exception("CompressFileInfo.FileName文件名为空");
            }
            string fileFullName = Path.Combine(base.filePath, Common.GetDirName(t8FileEntity.TaskSourceType));
            fileFullName = Path.Combine(fileFullName, Common.GetSubDirName(3),t8FileEntity.CompressFileInfo.FileName);
            return fileFullName;

        }

        public override string FileName(T8FileEntity t8FileEntity)
        {
            if (t8FileEntity.CompressFileInfo == null || string.IsNullOrEmpty(t8FileEntity.CompressFileInfo.FileName))
            {
                throw new Exception("CompressFileInfo.FileName文件名为空");
            }

            return t8FileEntity.CompressFileInfo.FileName;
        }
    }


    #endregion

    #region SQL条件查询时间
    /// <summary>
    /// SQL条件查询时间策略
    /// </summary>
    public class SqlQueryTimeStragety
    {
        private ISqlQueryTime sqlQueryTime;
        public SqlQueryTimeStragety(ISqlQueryTime sqlQueryTime)
        {
            this.sqlQueryTime = sqlQueryTime;
        }

        /// <summary>
        /// 得到开始时间 精确到秒
        /// </summary>
        /// <param name="basetime"></param>
        /// <returns></returns>
        public DateTime GetStartTime(DateTime basetime)
        {
            return this.sqlQueryTime.StartTime(basetime);
        }

        /// <summary>
        /// 得到结束时间 精确到秒
        /// </summary>
        /// <param name="basetime"></param>
        /// <returns></returns>
        public DateTime GetEndTime(DateTime basetime)
        {
            return this.sqlQueryTime.EndTime(basetime);
        }
    }

    /// <summary>
    /// SQL条件查询时间接口 
    /// </summary>
    public interface ISqlQueryTime
    {
        /// <summary>
        /// 生成查询开始时间
        /// </summary>      
        /// <returns></returns>
        DateTime StartTime(DateTime basetime);

        /// <summary>
        /// 生成查询结束时间
        /// </summary>    
        /// <returns></returns>
        DateTime EndTime(DateTime basetime);
    }

    /// <summary>
    /// 月查询起始时间(上月)
    /// </summary>
    public class MonthSqlQueryTime : ISqlQueryTime
    {  
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.LastMonthEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.LastMonthBegin(basetime);
        }
    }

    /// <summary>
    /// 周查询起始时间 （上周）
    /// </summary>
    public class WeekSqlQueryTime : ISqlQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.LastWeekEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.LastWeekBegin(basetime);
        }
    }

    /// <summary>
    /// 天查询起始时间 （昨天）
    /// </summary>
    public class DaySqlQueryTime : ISqlQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.LastDayEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.LastDayBegin(basetime);

        }
    }
    #endregion

    #region 定时条件查询时间

    /// <summary>
    /// 定时条件查询时间策略
    /// </summary>
    public class TimingQueryTimeStragety
    {
        private ITimingQueryTime timingQueryTime;
        public TimingQueryTimeStragety(ITimingQueryTime timingQueryTime)
        {
            this.timingQueryTime = timingQueryTime;
        }

        /// <summary>
        /// 得到开始时间 精确到秒
        /// </summary>
        /// <param name="basetime"></param>
        /// <returns></returns>
        public DateTime GetStartTime(DateTime basetime)
        {
            return this.timingQueryTime.StartTime(basetime);
        }

        /// <summary>
        /// 得到结束时间 精确到秒
        /// </summary>
        /// <param name="basetime"></param>
        /// <returns></returns>
        public DateTime GetEndTime(DateTime basetime)
        {
            return this.timingQueryTime.EndTime(basetime);
        }
    }

    /// <summary>
    /// 定时条件查询时间
    /// </summary>
    public interface ITimingQueryTime
    {
        /// <summary>
        /// 生成查询开始时间
        /// </summary>      
        /// <returns></returns>
        DateTime StartTime(DateTime basetime);

        /// <summary>
        /// 生成查询结束时间
        /// </summary>    
        /// <returns></returns>
        DateTime EndTime(DateTime basetime);
    }

    /// <summary>
    /// 月查询起始时间(上月)
    /// </summary>
    public class MonthTimingQueryTime : ITimingQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.MonthEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.MonthBegin(basetime);
        }
    }

    /// <summary>
    /// 周查询起始时间 （上周）
    /// </summary>
    public class WeekTimingQueryTime : ITimingQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.WeekEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.WeekBegin(basetime);
        }
    }

    /// <summary>
    /// 天查询起始时间 （昨天）
    /// </summary>
    public class DayTimingQueryTime : ITimingQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeMaster.DayEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeMaster.DayBegin(basetime);

        }
    }
    #endregion
}
