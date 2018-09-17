using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp2.Models;

namespace WebApp2
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //注册加EF服务
            //第一种 HelloWorldDBContext()构造函数设置
            //services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>(options => options.UseSqlite(Configuration["database:connection"]));

            //第二种
            services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>();

            //注册Identity服务
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HelloWorldDBContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseWelcomePage();

            //必须放在UseStaticFiles之前，否则出错
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            //UseFileServer  是对 app.UseDefaultFiles()和app.UseStaticFiles()的封装
            app.UseFileServer();

            //认证中间件一定要放在MVC中间件前
            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(ConfigureRoute);

            //app.Run(async (context) =>
            //{
            //    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //    //throw new Exception("Throw Exception");
            //    var msg = Configuration["message"];
            //    await context.Response.WriteAsync(HtmlEncoder.Default.Encode(msg));
            //});
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapRoute("Default","{controller=Home}/{action=Index}/{id?}");
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=index}/{id?}");
        }
    }
}
