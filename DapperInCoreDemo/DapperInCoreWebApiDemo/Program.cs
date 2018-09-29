using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DapperInCoreWebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .UseUrls("http://+9988")
                .UseStartup<Startup>();
    }
}