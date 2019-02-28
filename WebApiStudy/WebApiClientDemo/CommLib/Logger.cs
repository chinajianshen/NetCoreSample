using System.Text;
using Newtonsoft.Json;

namespace CommLib
{
    public static class Logger
    {
        public const string LOGTYPE_SYSINFO = "SYSINFO"; //系统信息
        public const string LOGTYPE_WARNING = "WARNING"; //警告
        public const string LOGTYPE_ERROR = "ERROR"; //错误或者异常

        public static LogWriter s_logger = new LogWriter();

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strLogType">信息，警告或错误</param>
        /// <param name="strLogCode">错误代码或模块名称</param>
        /// <param name="strContent">内容</param>
        /// <param name="values">参数</param>
        public static void Log(string strLogType, string strLogCode, string strContent, params object[] values)
        {
            s_logger.Log(strLogType, strLogCode, strContent, values);
        }

        /// <summary>
        /// 记录为Error文件，不会把StackTrace加进来
        /// </summary>
        public static void LogException2(string strLogCode, string strContent, params object[] values)
        {
            s_logger.Log(LOGTYPE_ERROR, strLogCode, strContent, values);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        public static void LogException2(string strLogCode, WebApiException ex)
        {
            StringBuilder sb = new StringBuilder();
            if (ex.InnerException != null)
            {
                sb.Append(ex.InnerException.Message);
                sb.Append("【");
                sb.Append(ex.InnerException.StackTrace);
                sb.Append("】");
            }
            else
            {
                sb.Append(ex.Message);
                sb.Append("【");
                sb.Append(ex.StackTrace);
                sb.Append("】");
            }
            if (ex.ParameterObject != null)
            {
                sb.Append("【");
                sb.Append(JsonConvert.SerializeObject(ex.ParameterObject));
                sb.Append("】");
            }
            s_logger.Log(LOGTYPE_ERROR, strLogCode, sb.ToString());
        }
    }
}
