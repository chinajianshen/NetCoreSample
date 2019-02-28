using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommLib
{
    public class Verifier
    {
        //用户名
        public const string REG_EXP_USER_NAME = @"^[0-9a-zA-Z_]{3,20}$";
        public const string ERRMSG_REG_EXP_USER_NAME = @"由字母、数字或下划线组成，3-20个字符";

        //中文名
        public const string REG_EXP_CHINESE_NAME = "^[\u4E00-\u9FA5]{2,25}$";
        public const string ERRMSG_REG_EXP_CHINESE_NAME = @"请填入正确的中文真实姓名";

        //密码
        public const string REG_EXP_PASSWORD = @"^[0-9a-zA-Z_\~\!\@\#\$\%\^\&\*\(\)\.\,\+\-\=]{3,20}$";
        public const string ERRMSG_REG_EXP_PASSWORD = @"密码必须由字母、数字或“~!@#$%^&*().,+-=”等特殊符号组成，3-20个字符";

        //验证相关的字符串
        public const string ERRMSG_CANNOT_BE_NULL = "不可为空";
        public const string ERRMSG_PASSWORDS_MUST_BE_EQUAL = "两次密码输入必须一致";
        public const string ERRMSG_EXCEED_MAX_LENGTH = "最多允许{1}个字符";

        
        //日志相关
        private const string REG_EXP_LOGGING_TYPE = @"^[0-9a-zA-Z_]{1,8}$";
        private static readonly Regex s_reLoggingType = new Regex(REG_EXP_LOGGING_TYPE);
        public static bool IsLegalLoggingType(string strLoggingType)
        {
            return s_reLoggingType.Match(strLoggingType).Success;
        }
        private const string REG_EXP_LOGGING_CODE = @"^[0-9a-zA-Z_]{1,20}$";
        private static readonly Regex s_reLoggingCode = new Regex(REG_EXP_LOGGING_CODE);
        public static bool IsLegalLoggingCode(string strLoggingCode)
        {
            return strLoggingCode == string.Empty || s_reLoggingCode.Match(strLoggingCode).Success;
        }
    }
}
