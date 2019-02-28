using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLib
{
    /// <summary>
    /// 用于客户端或局部的异常
    /// </summary>
    public class ClientException : Exception
    {
        public ClientException()
        {
        }

        /// <param name="message">对用户友好的信息</param>
        public ClientException(string message)
            : base(message)
        {
        }

        /// <param name="message">对用户友好的信息</param>
        /// <param name="inner">系统异常（用于记录）</param>
        public ClientException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// 产生异常的参数，利用反射将其所有Property记录下来
        /// </summary>
        public Object ParameterObject { get; set; }

        public const string ERRSTR_DATABASE = "数据库意外错误";
        public const string ERRSTR_ITEM_DOES_NOT_EXIST = "请求的条目不存在";
        public const string ERRSTR_ITEM_ALREADY_EXISTS = "该条目已存在";
    }
}
