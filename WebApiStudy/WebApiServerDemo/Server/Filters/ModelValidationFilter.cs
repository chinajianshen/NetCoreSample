using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommLib;

namespace Server.Filters
{
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        private const string INCORRECT_OR_MISSING_ARGUMENTS = "请求参数缺失或格式错误";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            if (actionContext.ActionArguments.Any(v => v.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new WebApiHttpInfo(INCORRECT_OR_MISSING_ARGUMENTS));
                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                List<String> errors = new List<String>();
                foreach (System.Web.Http.ModelBinding.ModelState ms in actionContext.ModelState.Values)
                {
                    foreach (System.Web.Http.ModelBinding.ModelError me in ms.Errors)
                    {
                        if (me.Exception != null)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new WebApiHttpInfo(INCORRECT_OR_MISSING_ARGUMENTS));
                            return;
                        }
                        else if (!string.IsNullOrEmpty(me.ErrorMessage))
                        {
                            errors.Add(me.ErrorMessage);
                        }
                    }
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, new WebApiHttpInfo(errors));
            }
        }
    }
}