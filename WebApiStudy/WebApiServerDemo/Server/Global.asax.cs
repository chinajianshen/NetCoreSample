using System.Web;
using System.Web.Http;
using CommLib;
using Server.Filters;

namespace Server
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            //Web API的过滤器增加在这里，而MVC的过滤器增加在FilterConfig.cs中
            GlobalConfiguration.Configuration.Filters.Add(new WebApiExceptionFilterAttribute()); //配置Web API各种异常的情况
            GlobalConfiguration.Configuration.Filters.Add(new ModelValidationFilterAttribute()); //发现Web API的ModelState错误并返回400

            Logger.s_logger.Init(Server.MapPath(@"~\log\"));

            //自动对象映射设置
            AutoMapperConfig.ConfigAutoMappings();

            //初始化全局Managers
            Managers.s_userManager.Load();
        }
    }
}