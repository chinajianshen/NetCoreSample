using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLib
{
    public enum WebApiExceptionCode
    {
        UnknownOrOther = 0,        //保留（不要使用）
        IncorrectArgument = 400,   //参数不正确 => 400
        Unauthorized = 401,        //未登录 => 401
        InsufficientRight = 403,   //权限不足 => 403
        ItemDoesNotExist = 404,    //要操作的条目不存在 => 404
        ConcurrencyConflict = 409, //并行写入冲突 => 409
        DatabaseError = 500,       //未知数据库错误 => 500
        InMaintaining = 503        //系统维护中 => 503
    }

    /// <summary>
    /// 用于服务器端的异常
    /// </summary>
    public class WebApiException : Exception
    {
        public WebApiException(WebApiExceptionCode code, Exception exOrg = null)
            : base(s_dictErrString[code], exOrg)
        {
            ExceptionCode = code;
        }

        public WebApiException(WebApiExceptionCode code, string strErrorMessage, Exception exOrg = null)
            : base(strErrorMessage, exOrg)
        {
            ExceptionCode = code;
        }

        public WebApiExceptionCode ExceptionCode { get; set; }
        public Object ParameterObject { get; set; } //参数对象，利用反射将其所有Property记录下来

        //默认异常信息
        private static readonly Dictionary<WebApiExceptionCode, string> s_dictErrString =
            new Dictionary<WebApiExceptionCode, string>
            {
                {WebApiExceptionCode.UnknownOrOther, "服务器意外错误，请与管理员联系"},
                {WebApiExceptionCode.IncorrectArgument, "请求参数不正确"},
                {WebApiExceptionCode.Unauthorized,"登录信息丢失，请刷新页面后重试"},
                {WebApiExceptionCode.InsufficientRight, "权限不足"},
                {WebApiExceptionCode.ItemDoesNotExist,"请求的条目不存在，请刷新页面后重试"},
                {WebApiExceptionCode.ConcurrencyConflict,"要操作的内容已经发生变动，请刷新页面后重试"},
                {WebApiExceptionCode.DatabaseError,"数据库意外错误"},
                {WebApiExceptionCode.InMaintaining,"系统维护中，请稍后再试"}
            };

        public static string GetDescriptionByCode(WebApiExceptionCode code)
        {
            return s_dictErrString[code];
        }
    }
}
