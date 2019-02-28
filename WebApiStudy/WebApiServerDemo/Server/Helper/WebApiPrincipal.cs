using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Http;
using CommLib;
using WebApiContract.Models;

namespace Server.Helper
{
    public class WebApiPrincipal : IPrincipal
    {
        private readonly WebApiIdentity m_identity;

        public WebApiPrincipal(WebApiIdentity identity)
        {
            m_identity = identity;
        }

        public IIdentity Identity
        {
            get
            {
                return m_identity;
            }
        }

        public bool IsInRole(string role)
        {
            return m_identity.Role == role;
        }
    }

    //此类包括一些最基本的用户信息
    public class WebApiIdentity : IIdentity
    {
        /// 表示用的验证方式是自定义验证
        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name { get; set; }

        public string DispName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }

    static public class ApiControlerExtension
    {
        //方便获取用户名的扩展方法
        static public string GetUserName(this ApiController controller)
        {
            if (controller.User is WebApiPrincipal)
            {
                return ((WebApiPrincipal)controller.User).Identity.Name;
            }
            else
            {
                return null;
            }
        }

        //方便获取用户密码（经过二次MD5加密）的扩展方法
        static public string GetUserTwiceMd5Pwd(this ApiController controller)
        {
            if (controller.User is WebApiPrincipal)
            {
                return ((WebApiIdentity)controller.User.Identity).Password;
            }
            else
            {
                return null;
            }
        }

        //方便获取用户角色的扩展方法
        static public string GetUserRole(this ApiController controller)
        {
            if (controller.User is WebApiPrincipal)
            {
                return ((WebApiIdentity)controller.User.Identity).Role;
            }
            else
            {
                return null;
            }
        }

        //确认id符合用户名的正则表达式
        static public void CheckUserName(this ApiController controller, string id)
        {
            if (!Regex.IsMatch(id, Verifier.REG_EXP_USER_NAME))
            {
                throw new WebApiException(WebApiExceptionCode.IncorrectArgument);
            }
        }

        //确认用户是Administrator或id是他自己
        static public void CheckAdministrator(this ApiController controller, string id)
        {
            if (controller.GetUserRole() != RoleType.ADMINISTARTOR && id != controller.GetUserName())
            {
                throw new WebApiException(WebApiExceptionCode.InsufficientRight);
            }
        }
    }
}
