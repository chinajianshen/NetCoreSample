using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using CommLib;
using Server.Filters;
using Server.Helper;
using WebApiContract.Models;
using WebApiKit;

namespace Server.Controllers
{
    [WebApiAuthFilter]
    public class PasswordController : ApiController
    {
        // PUT api/password/{username}
        // 用于修改自己的密码
        public void Put(string id, Password_API_Put password)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);

            //解密出明文密码
            string strPwdToSet = WebApiServerHelper.DecodeConfidentialMessage(password.Password, this.GetUserTwiceMd5Pwd());
            if (string.IsNullOrEmpty(strPwdToSet) || !Regex.IsMatch(password.Password, Verifier.REG_EXP_PASSWORD))
            {
                throw new WebApiException(WebApiExceptionCode.IncorrectArgument, Verifier.ERRMSG_REG_EXP_PASSWORD);
            }

            Managers.s_userManager.SetPassword(id, strPwdToSet);
        }
    }
}
