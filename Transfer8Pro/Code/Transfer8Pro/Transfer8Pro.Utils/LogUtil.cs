namespace Transfer8Pro.Utils
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    /// <summary>
    /// 日志记录类
    /// created by benny
    /// </summary>
    public class LogUtil
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
                            string str3 = "sp" + str2 + "all.log";
                            writer = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                            writer.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                            writer.Flush();
                            if (IsEveryThreadNeedOne)
                            {
                                str3 = "cm.Thread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
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
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        public static void WriteTaskLog(string Message, string TaskName)
        {
            WriteTaskLog(Message, TaskName, true);
        }
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        /// <param name="IsLogToDisk"></param>
        public static void WriteTaskLog(string Message, string TaskName, bool IsLogToDisk)
        {
            WriteTaskLog(Message, TaskName, IsLogToDisk, false);
        }
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        /// <param name="IsLogToDisk"></param>
        /// <param name="IsEveryThreadNeedOne"></param>
        public static void WriteTaskLog(string Message, string TaskName, bool IsLogToDisk, bool IsEveryThreadNeedOne)
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
                            string str3 = "t_" + TaskName + "_" + str2 + "all.log";
                            writer = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                            writer.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                            writer.Flush();
                            if (IsEveryThreadNeedOne)
                            {
                                str3 = "cm.Thread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
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
}

