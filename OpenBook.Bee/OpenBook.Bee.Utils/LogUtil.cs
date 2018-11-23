using log4net;
using Log4Simple.Core;
using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OpenBook.Bee.Utils
{
    /// <summary>
    /// 日志记录类
    /// created by benny
    /// </summary>
    public partial class LogUtil
    {
        public static string EXStr = "::OP\r\nEX:{0}\r\nSource:{1}\r\n{2}\r\n{3}\r\n::ED";
        private static object[] Locker = new object[0];

        public static void WriteLog(Exception ex)
        {
            WriteLog(string.Format(EXStr, new object[] { ex.Message, ex.Source, ex.StackTrace, ex.TargetSite }), true);
        }

        public static void WriteLog(string Message)
        {
            WriteLog(Message, true);
        }

        public static void WriteLog(string Message, bool IsLogToDisk)
        {
            WriteLog(Message, IsLogToDisk, false);
        }

        public static void WriteLog(string Message, bool IsLogToDisk, bool IsEveryThreadNeedOne)
        {
            if (IsLogToDisk)
            {
                lock (Locker)
                {
                    StreamWriter writer = null;
                    StreamWriter writer2 = null;
                    try
                    {
                        string logFolder = AppPath.LogFolder;
                        if (!string.IsNullOrEmpty(logFolder))
                        {
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str2 = DateTime.Now.ToString("yyyyMMdd");
                            logFolder = Path.Combine(logFolder, str2);
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str3 = string.Format("sp{0}all.log", str2);
                            writer = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                            writer.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                            writer.Flush();
                            if (IsEveryThreadNeedOne)
                            {
                                str3 = string.Format("cm.Thread{0}.log", Thread.CurrentThread.ManagedThreadId.ToString());
                                writer2 = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                                writer2.WriteLine(string.Format("{0}=>{1}", DateTime.Now.ToString(), Message));
                                writer2.Flush();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Close();
                        }
                        if (writer2 != null)
                        {
                            writer2.Close();
                        }
                    }
                }
            }
        }
    }


    public partial class LogUtil
    {
        static ILog logobj = LogManager.GetLogger("Log4Smart");


        public static void WriteLog(LogEntity log)
        {
            LoggingData logdata = new LoggingData();
            logdata.ExStringOne = log.LogTitle;
            logdata.Message = log.LogContent;
            logdata.ExIntTwo = log.LogTypeID;
            logdata.ExIntOne = log.UserID;
            logdata.ExStringTwo = log.LogMeta;
            if (HttpContext.Current != null)
            {
                logdata.ExStringThree = HttpContext.Current.Request.UserHostAddress;
            }
            logobj.Info(logdata);

#if DEBUG
            System.Diagnostics.Debug.WriteLine(LogEntity.ToJsonString(log));
#endif

        }


    }

    [Obsolete]
    public class LogUtil_Old
    {
        static readonly string LogDirKey = "LogDir";
        static readonly string LogDirKey_Raw = "LogDir_Raw";
        //static readonly string LogFileNameKey = "LogFileName";
        static object[] Locker = new object[0];
        /// <summary>
        /// 默认在调试模式下记录到控制台，磁盘，并分线程记载分日志；在Release下只记录到磁盘
        /// </summary>
        /// <param name="Message"></param>
        public static void WriteLog(string Message)
        {
            WriteLog(new LogEntity() { LogTitle = "普通系统日志", LogContent = Message, UserID = 0, LogTime = DateTime.Now, LogTypeID = (int)LogTypes.SysLog });
        }
        public static void WriteLog(Exception ex)
        {
            //WriteLog(string.Format(EXStr,ex.Message,ex.Source,ex.StackTrace,ex.TargetSite), true);
            WriteLog(new LogEntity() { UserID = 0, LogTitle = "系统异常", LogContent = ex.Message + ex.StackTrace, LogMeta = ex.Source, LogTime = DateTime.Now, LogTypeID = (int)LogTypes.SysError });
        }
        public static string EXStr = "::OP\r\nEX:{0}\r\nSource:{1}\r\n{2}\r\n{3}\r\n::ED";
        //        public static void WriteLog(string Message, bool IsLogToDisk)
        //        {
        //#if DEBUG
        //            WriteLog(Message, IsLogToDisk, true);
        //#else
        //            WriteLog(Message, IsLogToDisk, false);
        //#endif
        //        }
        public static void WriteLog(string Message, bool IsLogToDisk, bool IsRawLog)
        {
#if DEBUG
            Debug.WriteLine(Message);
#endif
            if (IsLogToDisk)
            {
                lock (Locker)
                {
                    System.IO.StreamWriter sw = null;
                    //System.IO.StreamWriter sw_thread = null;
                    try
                    {
                        string targetfile = PrepareLogFolder(IsRawLog);
                        if (string.IsNullOrEmpty(targetfile))
                        {
                            return;
                        }
                        sw = new StreamWriter(targetfile, true, Encoding.Unicode);
                        if (IsRawLog)
                        {
                            sw.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                        }
                        else
                        {
                            sw.WriteLine(Message);
                        }
                        sw.Flush();
                        //if (IsEveryThreadNeedOne)
                        //{
                        //    file = ConfigurationManager.AppSettings[LogFileNameKey] + "Thread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
                        //    sw_thread = new StreamWriter(Path.Combine(path, file), true, Encoding.Unicode);
                        //    sw_thread.WriteLine(string.Format("{0}=>{1}", DateTime.Now.ToString(), Message));
                        //    sw_thread.Flush();
                        //}
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        Debug.WriteLine(e);
#endif
                    }
                    finally
                    {
                        if (sw != null)
                            sw.Close();
                        //if (sw_thread != null)
                        //    sw_thread.Close();
                    }
                }
            }
        }

        private static string PrepareLogFolder(bool israwlog)
        {
            string path = israwlog ? ConfigurationManager.AppSettings[LogDirKey_Raw] : ConfigurationManager.AppSettings[LogDirKey];
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            string date = DateTime.Now.ToString("yyyyMMdd");
            if (israwlog)
            {
                return Path.Combine(path, string.Concat(date, ".log"));
            }
            else
            {
                path = Path.Combine(path, date);
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                return Path.Combine(path, string.Concat(date, DateTime.Now.Hour.ToString().PadLeft(2, '0'), ".log"));
            }
        }

        /// <summary>
        /// 原始日志，不会自动采集入库的日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteRawLog(string msg)
        {
            Console.WriteLine(string.Format("{0}=>{1}", DateTime.Now, msg));
#if DEBUG
            //Debug.WriteLine(msg);
#else
            WriteLog(msg, true, true);
#endif

        }
        public static void WriteRawLog(Exception ex)
        {
            WriteRawLog(string.Format(EXStr, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite));
        }
        /// <summary>
        /// 新版日志系统
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(LogEntity log)
        {
            log.UnqiueID = Guid.NewGuid();
            string data = LogEntity.ToJsonString(log);
            WriteLog(string.Concat("START==>\r\n", data, "\r\n==>END\r\n"), true, false);
        }
    }
}
