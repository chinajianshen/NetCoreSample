using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommLib;

namespace Server.Filters
{
    public class WebApiRoleFilterAttribute : ActionFilterAttribute
    {
        public WebApiRoleFilterAttribute(string strRole)
        {
            Role = strRole;
        }

        public string Role { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (HttpContext.Current.User.IsInRole(Role))
                return;
            throw new WebApiException(WebApiExceptionCode.InsufficientRight);
        }
    }
}