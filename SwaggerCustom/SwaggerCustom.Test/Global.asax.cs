using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SwaggerCustom.Test.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SwaggerCustom.Test
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new JsonResultFilter());
            GlobalConfiguration.Configure(WebApiConfig.Register);         
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {


            if (Request.Url.ToString().Contains("swagger/docs/v1"))
            {
                return;
            }
            //bool isAjax = new HttpRequestWrapper(Request).IsAjaxRequest();
            //if (isAjax)
            //{
            //    var token =  Request.Headers["t8_token"].FirstOrDefault();
            //    var tick = Request.Headers["t8_tick"].FirstOrDefault();

            //    Response.Clear();
            //    Response.ContentType = "application/json; charset=utf-8";
            //    Response.Write(ToJson(
            //    new 
            //    {
            //        Status = -1,
            //        Msg = "调用签名错误"
            //    }));
            //    Response.Flush();              
            //}
        }

        public static string ToJson(object o)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }
    }
}
