using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 返回Json数据类型
    /// </summary>
    public class JsonResponse
    {
        public JsonResponse()
        {
            succeed = false;
            message = "未知错误";
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool succeed { get; set; }

        /// <summary>
        /// 操作结果详细代码【必要时】
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 操作结果消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 操作产生的数据【必要时】
        /// </summary>
        public dynamic Data { get; set; }
    }
}
