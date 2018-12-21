using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Transfer8Pro.Utils
{
   public class RegexExpressionUtil
    {
        /// <summary>
        /// 验证是否是文件路径格式 如 C:\Users
        /// </summary>
        public static Regex ValidateFilePathReg = new Regex(@".*?\w{1}:\.*?", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
