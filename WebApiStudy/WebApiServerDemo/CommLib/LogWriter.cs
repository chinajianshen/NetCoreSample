using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommLib
{
    public class LogWriter
    {
        //内部类
        protected class LogFileInfo
        {
            public FileStream m_fsLogFile = null; //文件流
            public DateTime m_dtLastAction = DateTime.Now - TimeSpan.FromDays(1); //最后写的时间
        }

        public LogWriter()
        {
            if (m_bOneInstanceCreated)
            {
                throw new ClientException("LogWriter只允许一个实例，建议把LogWriter对象定义为静态的.");
            }
            m_bOneInstanceCreated = true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="strLogPath">Log文件的路径</param>
        public void Init(string strLogPath)
        {
            if (strLogPath.LastIndexOf('\\') != strLogPath.Length - 1)
                strLogPath += "\\";
            if (!Directory.Exists(strLogPath))
                Directory.CreateDirectory(strLogPath);
            m_strLogPath = strLogPath;
            m_bInitialized = true;
        }

        /// <summary>
        /// 关闭所有文件
        /// </summary>
        public void Close()
        {
            foreach (LogFileInfo lfi in m_dictLogFiles.Values)
            {
                if (lfi.m_fsLogFile != null)
                {
                    lfi.m_fsLogFile.Close();
                    lfi.m_fsLogFile = null;
                }
            }
        }

        public void Log(string strLogType, string strLogCode, string strContent, params object[] values)
        {
            if (!Verifier.IsLegalLoggingType(strLogType)
                || !Verifier.IsLegalLoggingCode(strLogCode))
            {
                throw new ClientException("日志记录的参数不正确.");
            }

            if (!m_bInitialized)
            {
                throw new ClientException("日志记录对象未正确初始化.");
            }

            LogFileInfo lfi;
            if (m_dictLogFiles.ContainsKey(strLogType))
                lfi = m_dictLogFiles[strLogType];
            else
            {
                lfi = new LogFileInfo();
                m_dictLogFiles.Add(strLogType, lfi);
            }

            DateTime dtLogFile = DateTime.Now;
            if (dtLogFile.Day != lfi.m_dtLastAction.Day)
            {
                //如果文件流已经打开，就关闭它
                if (lfi.m_fsLogFile != null)
                    lfi.m_fsLogFile.Close();

                //如果目录不存在，创建之
                string strLogPath = LogHelper.GetLogSubFolder(m_strLogPath, dtLogFile);
                if (!Directory.Exists(strLogPath))
                    Directory.CreateDirectory(strLogPath);

                //如果Log文件已存在，就以追加方式打开它
                //如果Log文件不存在，就创建它
                string strLogFile = LogHelper.GetLogFileName(m_strLogPath, strLogType, dtLogFile);
                if (File.Exists(strLogFile))
                {
                    lfi.m_fsLogFile = File.Open(strLogFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                else
                {
                    lfi.m_fsLogFile = File.Open(strLogFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                    byte[] bom = Encoding.UTF8.GetPreamble();
                    lfi.m_fsLogFile.Write(bom, 0, bom.Length);
                }
            }

            //写内容
            StringBuilder sb = new StringBuilder();
            sb.Append(LogHelper.DtToString(DateTime.Now));
            sb.Append(" ");
            sb.Append(strLogCode);
            sb.Append(" ");
            if (values.Any())
                sb.AppendFormat(strContent, values);
            else
                sb.Append(strContent);
            sb.Replace("\r\n", "\t");
            sb.Replace('\r', '\t');
            sb.Replace('\n', '\t');
            sb.Append("\r\n");
            byte[] byToWrite = Encoding.UTF8.GetBytes(sb.ToString());
            lock (thisLock)
            {
                lfi.m_fsLogFile.Write(byToWrite, 0, byToWrite.Length);
                lfi.m_fsLogFile.Flush();
            }

            lfi.m_dtLastAction = DateTime.Now;
        }

        protected Dictionary<string, LogFileInfo> m_dictLogFiles = new Dictionary<string, LogFileInfo>();
        //可帮助用LogType来获到LogFileInfo

        protected string m_strLogPath; //Log根目录，末尾带上反斜杠
        protected bool m_bInitialized = false; //是否已经初始化
        protected static bool m_bOneInstanceCreated = false; //已创建一个实例
        private readonly Object thisLock = new Object();
    }
}
