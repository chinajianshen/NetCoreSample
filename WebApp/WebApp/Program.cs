using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //NLog.Web.NLogBuilder.ConfigureNLog("nlog1.config");  //假如没有用默认的名字，多写了一个1
            CreateWebHostBuilder(args).Build().Run();

            //var builder = new WebHostBuilder().UseKestrel().UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureAppConfiguration((hostingContext,config)=>
            //    {
            //        var env = hostingContext.HostingEnvironment;
                    
            //    });
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)               
                .UseStartup<Startup>();


    // ConfigureLogging(build =>
    //{
    //        build.AddFilter(f => f == LogLevel.Debug);
    //        build.AddEventSourceLogger();
    //    })
        //配置第三方NLog
        //.ConfigureLogging(logging =>
        //    {
        //        logging.ClearProviders();
        //        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //设置最小的日志级别
        //    }).UseNLog();
    }
}
