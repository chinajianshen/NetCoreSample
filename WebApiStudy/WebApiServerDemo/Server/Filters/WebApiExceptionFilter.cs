using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using CommLib;

namespace Server.Filters
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            const string MODEL_NAME_EXCEPTION_FILTER = "ExceptionFilter";

            HttpStatusCode code;
            string message;
            if (context.Exception is WebApiException)
            {
                WebApiException ex = (WebApiException)context.Exception;
                switch (ex.ExceptionCode)
                {
                    case WebApiExceptionCode.IncorrectArgument: //参数错误（400，通常不会在这里捕捉到，因为会有另一个过滤器处理）
                        code = HttpStatusCode.BadRequest;
                        break;
                    case WebApiExceptionCode.Unauthorized: //身份验证失败(401)
                        code = HttpStatusCode.Unauthorized;
                        break;
                    case WebApiExceptionCode.ItemDoesNotExist:  //找不到资源（404）
                        code = HttpStatusCode.NotFound;
                        break;
                    case WebApiExceptionCode.InsufficientRight: //权限不足（403，如果被权限过滤器处理过，则不会在这里被捕捉到）
                        code = HttpStatusCode.Forbidden;
                        break;
                    case WebApiExceptionCode.ConcurrencyConflict: //并行写入冲突
                        code = HttpStatusCode.Conflict;
                        break;
                    default:
                        code = HttpStatusCode.InternalServerError; //其它错误（500，一般是数据库错误，或者一些业务逻辑错误）
                        if (ex.InnerException != null)
                        {
                            Logger.LogException2(MODEL_NAME_EXCEPTION_FILTER, ex);
                        }
                        break;
                }
                message = context.Exception.Message;
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
                Logger.LogException2(MODEL_NAME_EXCEPTION_FILTER, context.Exception.Message + "【" + context.Exception.StackTrace + "】"); //Log一些控制之外的错误
                message = "服务器意外错误";
            }
            context.Response = context.Request.CreateResponse(code, new WebApiHttpInfo(message));
        }
    }
}