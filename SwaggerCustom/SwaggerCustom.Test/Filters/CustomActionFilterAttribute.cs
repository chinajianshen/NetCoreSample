using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SwaggerCustom.Test.Filters
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //if (!actionContext.ModelState.IsValid)
            //{
            //    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            //}
            var token = actionContext.Request.Headers.GetValues("t8_token").FirstOrDefault();
            var tick = actionContext.Request.Headers.GetValues("t8_tick").FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                PreUnauthorized(actionContext);
                return;
            }
        }

        private void PreUnauthorized(HttpActionContext actionContext)
        {
            // 如果用户没有登录，则返回一个通用的错误Model
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "验证失败");
        }
    }
}