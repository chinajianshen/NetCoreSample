using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NineskyStudy.Base;
using NineskyStudy.Hubs;
using NineskyStudy.Infrastructure;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;
using UEditor.Core;

namespace NineskyStudy
{
    public class Startup
    {
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            //ContentRootPath  用于包含应用程序文件如C:\MyApp\
            //WebRootPath 用于包含Web服务性的内容文件C:\MyApp\wwwroot\
            _contentRootPath = env.ContentRootPath;

            #region 测试
            //List<AssemblyItem> items = new List<AssemblyItem>() {
            //     new AssemblyItem{
            //          ServiceAssembly =" NineskyStudy.InterfaceBase",
            //          ImplementationAssembly="NineskyStudy.Base.dll",
            //          DICollections = new List<ServiceItem>
            //          {
            //              new ServiceItem{ ServiceType="NineskyStudy.InterfaceBase.InterfaceCategoryService",
            //                  ImplementationType ="NineskyStudy.Base.CategoryService",LifeTime= ServiceLifetime.Scoped}
            //          }
            //     },
            //      new AssemblyItem{
            //          ServiceAssembly =" NineskyStudy.InterfaceBase2",
            //          ImplementationAssembly="NineskyStudy.Base2.dll",
            //          DICollections = new List<ServiceItem>
            //          {
            //              new ServiceItem{ ServiceType="NineskyStudy.InterfaceBase.InterfaceCategoryService2",
            //                  ImplementationType ="NineskyStudy.Base.CategoryService2",LifeTime= ServiceLifetime.Scoped}
            //          }
            //     }
            //};
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            #endregion

            // 构造函数注入IConfiguration，相当于下面操作
            //var builder = new ConfigurationBuilder()
            //              .SetBasePath(env.ContentRootPath)
            //              .AddJsonFile("");
            //Configuration = builder.Build();

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

            string conn = Configuration.GetConnectionString("DefaultConnection");
            //数据库连接串 采用附加库形式 https://www.cnblogs.com/chonghanyu/p/5709780.html
            if (conn.Contains("%CONTENTROOTPATH%"))
            {
                conn = conn.Replace("%CONTENTROOTPATH%", _contentRootPath);
            }

            //注入NineskyDbContext 参照 https://www.cnblogs.com/mzwhj/p/6147900.html 第4部分
            services.AddDbContext<NineskyDbContext>(options =>
            {
                options.UseSqlServer(conn);
            });
            //数据库与接口注入
            services.AddScoped<DbContext, NineskyDbContext>();

            #region 数据库和业务逻辑服务注入
            //services.AddScoped<InterfaceBaseRepository<Category>, BaseRepository<Category>>();

            //自己添加接口注入
            //services.AddScoped<InterfaceBaseService<Category>, BaseService<Category>>();
            //services.AddScoped<InterfaceCategoryService, CategoryService>();           

            //services.AddScoped<CategoryService>();
            #endregion

            #region 配置文件动态注入 参考：https://www.cnblogs.com/mzwhj/p/6224237.html
            var assemblyCollections = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("service.json").Build().GetSection("AssemblyCollections").Get<List<AssemblyItem>>();
            foreach (var assembly in assemblyCollections)
            {
                //加载接口程序集使用的方法是Assembly.Load(new AssemblyName(assembly.ServiceAssembly))，这是因为项目引用了接口程序集的项目，加载程序集的时候只需要提供程序集的名称就可以
                var serviceAssembly = Assembly.Load(new AssemblyName(assembly.ServiceAssembly));
                //加载实现类所在程序集的时候使用的是AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + "//" + assembly.ImplementationAssembly)。在.Net Core中Assembly没有了LoadFrom方法，仅有一个Load方法加载已引用的程序集。多方搜索资料才找到AssemblyLoadContext中有一个方法可以不需要引用项目可以动态加载Dll，但必须包含Dll的完整路径。
                var implementationAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + assembly.ImplementationAssembly);
                foreach (var service in assembly.DICollections)
                {
                    services.Add(new ServiceDescriptor(serviceAssembly.GetType(service.ServiceType),
                           implementationAssembly.GetType(service.ImplementationType), service.LifeTime));
                }
            }
            #endregion

            //注册百度uEdtior编辑器
            services.AddUEditorService();          

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //注册SingalR服务
            services.AddSignalR();

            //注册自定义筛选服务（执行顺序 中间件处理前后）
            services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //顺序1
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }           

            //程序2
            app.UseHttpsRedirection();

            //顺序3
            //配置TypeScript创建SignalR Web应用  https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/signalr-typescript-webpack?view=aspnetcore-2.1&tabs=visual-studio
            app.UseDefaultFiles();
           
            //app.UseStaticFiles();
            //配置uEdtior编辑器
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(), "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });           

            //顺序4
            app.UseCookiePolicy();

            //
            app.UseSignalR(routes =>
            {
                //routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<ChatHub>("/hub");
            });

            //顺序5
            //app.UseAuthentication();

            //app.Map("/map1", HandleMapTest1);
            //app.Map("/map2", HandleMapTest2);
            //app.MapWhen(context => context.Request.Query.ContainsKey("branch"),HandlerBranch);


            //顺序6
            app.UseMvc(routes =>
            {
                //注册一个区域
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute("blog", "blog/{*article}", defaults: new { controller="Blog",action="Article" });
                               

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            #region 配置自定义中间件
            //直接Use定义中间件 设置当前请求区域性
            //app.Use(async (context, next) =>
            //{
            //    await next.Invoke();               
            //});

            //app.Use((context,next) => {
            //    var cultureQuery = context.Request.Query["culture"];
            //    if (!string.IsNullOrWhiteSpace(cultureQuery))
            //    {
            //        var culture = new CultureInfo(cultureQuery);
            //        CultureInfo.CurrentCulture = culture;
            //        CultureInfo.CurrentUICulture = culture;
            //    }

            //    return next();
            //});

            //app.UseRequestCultrue();
            #endregion

            //初始化数据库
            //Models.DbInitializer.Initialize();
        }

        #region 测试
        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }

        private static void HandlerBranch(IApplicationBuilder app)
        {
            app.Run(async context => {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used={branchVer}");
            });
        }
        #endregion
    }
}
