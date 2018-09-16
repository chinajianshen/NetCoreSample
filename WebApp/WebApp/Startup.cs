using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<MyOptions>(Configuration.GetSection("MyOptions"));
            //services.AddSingleton<MyOptions>()

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //注册自定义的认证服务 （没有完成 以后再完成）
            //services.AddAuthenticationCore(options => {
            //    options.AddScheme<MyHandler>("myScheme", "demo scheme");

            //});

            // services.AddAuthenticationCore();

            //注册自定义定时服务
            //services.AddSingleton<IHostedService, TokenRefreshService>();

            //注册自定义服务2
            //services.AddSingleton<IHostedService, TokenRefreshBackgroundService>();

            //services.AddLogging(builder =>
            //{
            //    builder
            //        .AddConfiguration(loggingConfiguration.GetSection("Logging"))
            //        .AddFilter("Microsoft", LogLevel.Warning)
            //        .AddConsole();
            //});

            //DI中注册服务认证所需的服务
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(o =>
            {
                o.ClientId = "server.hybrid";
                o.ClientSecret = "secret";
                o.Authority = "https://demo.identityserver.io/";
                o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            });
        }

        //构建Http请求管道
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            //app.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        await context.Response.WriteAsync("Hello ASP.NET Core!");                    
            //    };
            //});       

            app.UseAuthentication();

            //配置自定义中间件
            //app.UseFloorOne();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseCookieAuthentication(new CookieAuthenticationOptions() {
            //    //AuthenticationScheme = "myuser", //名称
            //    //AutomaticAuthenticate = true,//自动验证
            //    LoginPath = "/account/login"//登录地址

            //});
            
        }

        //替换NetCore默认DI框架
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.RegisterModule(new AutofacModule());
        //}

        //public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory
        //        .AddFilter("Microsoft", LogLevel.Warning)
        //        .AddFilter("System", LogLevel.Warning)
        //        .AddFilter("SampleApp.Program", LogLevel.Debug)
        //        .AddDebug();
        //}
    }
}
