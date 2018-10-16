using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NineskyStudy.Infrastructure;

namespace NineskyStudy
{
    public class Program
    {
        public static Dictionary<string, string> arrayDict = new Dictionary<string, string> {
            {"array:entries:0", "value0"},
            {"array:entries:1", "value1"},
            {"array:entries:2", "value2"},
            {"array:entries:4", "value4"},
            {"array:entries:5", "value5"}
        };
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //默认启动
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        //根据当前启动环境 加载Startup StartupDevelopment StartupProduction
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    var assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;
        //    return WebHost.CreateDefaultBuilder(args).UseStartup(assemblyName);
        //}

        private static string GetDbConn()
        {
            //构造函数注入IConfiguration，相当于下面操作
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");
            string conn = builder.Build().GetConnectionString("DefaultConnection");
            return conn;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       config.SetBasePath(Directory.GetCurrentDirectory());
                       config.AddInMemoryCollection(arrayDict);
                       //config.AddJsonFile("json_array.json", optional: false, reloadOnChange: false);
                       //config.AddXmlFile("tvshow.xml", optional: false, reloadOnChange: false);
                       //config.AddEFConfiguration(options => options.UseInMemoryDatabase("InMemoryDb"));
                       config.AddEFConfiguration(options => options.UseSqlServer(GetDbConn()));

                       config.AddCommandLine(args);

                   })
                  .UseStartup<Startup>();
    }
}
