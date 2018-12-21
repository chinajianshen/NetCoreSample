using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.Infrastructure
{
    public class TaskFileNameManager
    {
    }

    #region 生成文件名及路径
    /// 生成文件名及路径
    /// </summary>
    public abstract class AFileName
    {
        protected readonly string filePath;
        protected string fileName;

        public AFileName()
        {
            filePath = AppPath.DataFolder;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        /// <summary>
        /// 文件名
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public abstract string FileName(TaskEntity taskEntity);

        /// <summary>
        /// 文件完全路径
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public abstract string FileFullName(TaskEntity taskEntity);
    }

    /// <summary>
    /// 一般文件名称
    /// </summary>
    public class GeneralFileName : AFileName
    {
        private string fileName;
        public override string FileFullName(TaskEntity taskEntity)
        {
            string normalFilePath = Path.Combine(base.filePath, ConfigHelper.GetConfig("NormalFilePath", "NormalDataFile"));
            if (!Directory.Exists(normalFilePath))
            {
                Directory.CreateDirectory(normalFilePath);
            }

            string fileName = FileName(taskEntity);

            return Path.Combine(normalFilePath, fileName);
        }

        /// <summary>
        /// 一般文件名格式 20181012_093424_d_book.jl     
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public override string FileName(TaskEntity taskEntity)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Format("{0}_{1}_{2}_{3}.jl"
                    , DateTime.Now.ToString("yyyyMMdd")
                    , DateTime.Now.ToString("HHmmss")
                    , taskEntity.CycleType.ToString().ToLower()
                    , taskEntity.DataType.ToString().ToLower());
            }

            return fileName;
        }
    }

    /// <summary>
    /// 压缩文件名称
    /// </summary>
    public class CompressFileName : AFileName
    {
        private string fileName;
        private Regex normalFileValidateReg = new Regex(@"^\d{8}_\d{6}_\w{1}_\w{4,10}.jl$", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public override string FileFullName(TaskEntity taskEntity)
        {
            string filePath = Path.Combine(base.filePath, ConfigHelper.GetConfig("CompressFilePath", "CompressDataFile"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileName = FileName(taskEntity);

            return Path.Combine(filePath, fileName);
        }

        public override string FileName(TaskEntity taskEntity)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            if (taskEntity.FileInfo == null || string.IsNullOrEmpty(taskEntity.FileInfo.NormalFilePath))
            {
                throw new ArgumentNullException("生成压缩文件时，一般文件地址为空，无法生成压缩文件");
            }

            if (!File.Exists(taskEntity.FileInfo.NormalFilePath))
            {
                throw new Exception(string.Format("生成压缩文件时，一般文件[{0}]指定路径下不存在，无法生成压缩文件", taskEntity.FileInfo.NormalFilePath));
            }

            string compressPath = FileHelper.GetDirectoryName(taskEntity.FileInfo.NormalFilePath);
            string compressFileName = FileHelper.GetFileName(taskEntity.FileInfo.NormalFilePath);
            string compressFileNameWithoutExt = FileHelper.GetFileNameWithoutExtension(taskEntity.FileInfo.NormalFilePath);

            if (!normalFileValidateReg.IsMatch(compressFileName))
            {
                throw new Exception(string.Format("生成压缩文件时，一般文件名[{0}]格式不符合要求", compressFileName));
            }

            string encryptKey = Common.GetEncryptKey();
            if (string.IsNullOrEmpty(encryptKey))
            {
                throw new Exception("生成压缩文件时，配置中未读取密钥串");
            }

            string fileMd5 = MD5Utils.GetMD5HashFromFile(taskEntity.FileInfo.NormalFilePath);

            string fileAndKeyMd5 = MD5Utils.GetMD5(fileMd5 + encryptKey);

            //文件校验位 按照规则前三个字符和后三个字符拼接组成6个字符作为校验码
            string fileDataCode = fileAndKeyMd5.PreNChar(3) + fileAndKeyMd5.AfterNChar(3);

            if (string.IsNullOrEmpty(fileDataCode) || fileDataCode.Length != 6)
            {
                throw new Exception("生成压缩文件时，计算文件校验位时出现异常");
            }


            compressFileNameWithoutExt += "_" + fileDataCode;

            string fileNameMd5 = MD5Utils.GetMD5(compressFileNameWithoutExt + "_" + encryptKey);

            //生成文件校验位 按照规则取后2个字符和前4个字符拼接成6个字符为校验位
            string fileNameCode = fileNameMd5.AfterNChar(2) + fileNameMd5.PreNChar(4);


            if (string.IsNullOrEmpty(fileNameCode) || fileNameCode.Length != 6)
            {
                throw new Exception("生成压缩文件时，计算文件名校验位时出现异常");
            }

            fileName = compressFileNameWithoutExt + "_" + fileNameCode + ".jl" + ".zip";
            return fileName;
        }
    }

    /// <summary>
    /// 传8一般文件名称
    /// </summary>
    public class T8GeneralFileName : AFileName
    {
        private string fileName;
        public override string FileFullName(TaskEntity taskEntity)
        {
            string normalFilePath = Path.Combine(base.filePath, ConfigHelper.GetConfig("NormalFilePath", "NormalDataFile"));
            if (!Directory.Exists(normalFilePath))
            {
                Directory.CreateDirectory(normalFilePath);
            }

            string fileName = FileName(taskEntity);

            return Path.Combine(normalFilePath, fileName);
        }

        public override string FileName(TaskEntity taskEntity)
        {
            //周 t8sjzch_20051226_20060101_W.db
            //周在架 t8sjzch_20051226_20060101_WS.db
            //月 t8sjzch_20160601_20160630_M.db
            //月在架 t8sjzch_20160525_20160525_S.db

            if (string.IsNullOrEmpty(fileName))
            {
                string startTime = "";
                string endTime = "";
                string cycleType = "";

                ISqlQueryTime sqlQueryTime = new BuildInstanceObject().GetSqlQueryTimeStragety(taskEntity.CycleType);
                switch (taskEntity.DataType)
                {
                    case DataTypes.Sale:                      
                        switch (taskEntity.CycleType)
                        {
                            case CycleTypes.W:
                                cycleType = $"{CycleTypes.W}";

                                break;
                            case CycleTypes.M:
                                cycleType = $"{CycleTypes.M}";
                                break;
                            default:
                                throw new Exception($"传8任务[{taskEntity.TaskID}]日期类型[{taskEntity.CycleType}]设置有误，必须设置日期类型为周或月");
                        }

                        startTime = sqlQueryTime.StartTime(DateTime.Now).ToString("yyyyMMdd");
                        endTime = sqlQueryTime.EndTime(DateTime.Now).ToString("yyyyMMdd");
                        break;
                    case DataTypes.Stock:
                        switch (taskEntity.CycleType)
                        {
                            case CycleTypes.W:
                                cycleType = $"{CycleTypes.W}S";

                                startTime = sqlQueryTime.StartTime(DateTime.Now).ToString("yyyyMMdd");
                                endTime = sqlQueryTime.EndTime(DateTime.Now).ToString("yyyyMMdd");
                                break;
                            case CycleTypes.M:
                                cycleType = "S";

                                startTime = DateTime.Now.ToString("yyyyMMdd");
                                endTime = DateTime.Now.ToString("yyyyMMdd");
                                break;
                            default:
                                throw new Exception($"传8任务[{taskEntity.TaskID}]日期类型[{taskEntity.CycleType}]设置有误，必须设置日期类型为周或月");
                        }
                        break;
                    default:
                        throw new Exception($"传8任务[{taskEntity.TaskID}]数据类型[{taskEntity.DataType}]设置有误，必须设置销售或在架");
                }

                fileName = string.Format("{0}_{1}_{2}_{3}.db", taskEntity.FtpConfig.UserName,startTime,endTime,cycleType);
            }

            return fileName;
        }
    }

    /// <summary>
    /// 传8压缩文件名称
    /// </summary>
    public class T8CompressFileName : AFileName
    {
        private string fileName;
        public override string FileFullName(TaskEntity taskEntity)
        {
            string filePath = Path.Combine(base.filePath, ConfigHelper.GetConfig("CompressFilePath", "CompressDataFile"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileName = FileName(taskEntity);

            return Path.Combine(filePath, fileName);
        }

        public override string FileName(TaskEntity taskEntity)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            if (taskEntity.FileInfo == null || string.IsNullOrEmpty(taskEntity.FileInfo.NormalFilePath))
            {
                throw new ArgumentNullException("生成压缩文件时，一般文件地址为空，无法生成压缩文件");
            }

            if (!File.Exists(taskEntity.FileInfo.NormalFilePath))
            {
                throw new Exception(string.Format("生成压缩文件时，一般文件[{0}]指定路径下不存在，无法生成压缩文件", taskEntity.FileInfo.NormalFilePath));
            }

            fileName = Path.Combine(FileHelper.GetFileName(taskEntity.FileInfo.NormalFilePath), ".zip");
            return fileName;
        }
    }
    #endregion

    #region SQL条件查询时间
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
            return DateTimeHelper.LastMonthEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeHelper.LastMonthBegin(basetime);
        }
    }

    /// <summary>
    /// 周查询起始时间 （上周）
    /// </summary>
    public class WeekSqlQueryTime : ISqlQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeHelper.LastWeekEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeHelper.LastWeekBegin(basetime);
        }
    }

    /// <summary>
    /// 天查询起始时间 （昨天）
    /// </summary>
    public class DaySqlQueryTime : ISqlQueryTime
    {
        public DateTime EndTime(DateTime basetime)
        {
            return DateTimeHelper.LastDayEnd(basetime);
        }

        public DateTime StartTime(DateTime basetime)
        {
            return DateTimeHelper.LastDayBegin(basetime);

        }
    }
    #endregion

}
