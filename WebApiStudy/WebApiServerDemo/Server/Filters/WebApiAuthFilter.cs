using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using BLL;
using Server.Helper;
using WebApiKit;
using CommLib;

namespace Server.Filters
{
    /// <summary>
    /// 身份验证过滤器
    /// </summary>
    public class WebApiAuthFilterAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains(Consts.HTTP_HEADER_AUTH_USER) && actionContext.Request.Headers.Contains(Consts.HTTP_HEADER_AUTH_KEY))
            {
                IEnumerable<string> arrCustomAuthName = actionContext.Request.Headers.GetValues(Consts.HTTP_HEADER_AUTH_USER);
                IEnumerable<string> arrCustomAuthKey = actionContext.Request.Headers.GetValues(Consts.HTTP_HEADER_AUTH_KEY);
                if (arrCustomAuthName.Any() && arrCustomAuthKey.Any())
                {
                    WebApiPrincipal principal = GetWebApiPrincipal(arrCustomAuthName.First(), arrCustomAuthKey.First(), actionContext);
                    if (principal != null)
                    {
                        HttpContext.Current.User = principal;
                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
            //判断用户是否登录
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new WebApiException(WebApiExceptionCode.Unauthorized, "身份验证失败");
        }

        protected WebApiPrincipal GetWebApiPrincipal(string strName, string strKey, HttpActionContext actionContext)
        {
            //获取用户基本信息（包括经过二次MD5加密的密码）
            UserInfo_BLL userBll = Managers.s_userManager.GetUser(strName);
            if (userBll != null)
            {
                string strEncryptedPassword = Managers.s_userManager.GetEncryptedPwdOfUser(strName);
                try
                {
                    Guid guidRequest = Guid.Empty;
                    if (!WebApiServerHelper.VerifyAuthKey(strName, strKey, actionContext.Request.RequestUri.ToString(),
                                                     strEncryptedPassword, ref guidRequest))
                        return null;

                    //判断GUID防止重发攻击
                    if (!GlobalServerData.s_guidsetRequest.IsExistAndAdd(guidRequest))
                        return null;

                    return new WebApiPrincipal(new WebApiIdentity
                    {
                        Name = userBll.UserName,
                        DispName = userBll.RealName,
                        Password = strEncryptedPassword,
                        Role = userBll.Role
                    });
                }
                catch (Exception)
                {
                    //Ignore any exception
                }
            }
            return null;
        }
    }
}