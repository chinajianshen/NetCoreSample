using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NineskyStudy.AutoMapperConfig;
using NineskyStudy.Base;
using NineskyStudy.Hubs;
using NineskyStudy.Infrastructure;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using UEditor.Core;

namespace NineskyStudy
{
    public class Startup
    {
        private string _contentRootPath = "";
        private Container _container;
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

            //注册AutoMapper服务
            services.AddAutoMapper();

            //自定义视图路径方法1 .AddRazorOptions（）中设置 (此方法只在应用程序启动后初始化一次)
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //.AddRazorOptions(opt =>
            //{
            //    opt.ViewLocationFormats.Clear();//清空默认列表
            //    opt.ViewLocationFormats.Add("~/Views/{1}/{0}.cshtml");
            //    opt.ViewLocationFormats.Add("~/Views/Shared/{0}.cshtml");
            //    opt.ViewLocationFormats.Add("~/Views/{1}/Template/{0}.cshtml");

            //    opt.AreaViewLocationFormats.Clear();
            //    opt.AreaViewLocationFormats.Add("~/Areas/{2}/Views/{1}/{0}.cshtml");
            //    opt.AreaViewLocationFormats.Add("~/Areas/{2}/Views/Shared/{0}.cshtml");
            //    opt.AreaViewLocationFormats.Add("~/Areas/{2}/Views/{1}/Template/{0}.cshtml");
            //});

            //自定义视图路径方法2 （此方法每次视图页面执行都会执行 性能会有影响）
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new TemplateViewLocationExPander());
                //options.AreaViewLocationFormats.Add();
            });


            #region Simple Injector第三方DI容器替换原有的     
            //https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/extensibility-third-party-container?view=aspnetcore-2.1
            //services.AddTransient<IMiddlewareFactory>(_ =>
            //{
            //    return new SimpleInjectorMiddlewareFactory(_container);
            //});          

            //services.UseSimpleInjectorAspNetRequestScoping(_container);

            //services.AddScoped<NineskyDbContext>(provider =>
            //    _container.GetInstance<NineskyDbContext>());

            //_container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //_container.Register<NineskyDbContext>(() =>
            //{
            //    var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            //    optionsBuilder.UseInMemoryDatabase("InMemoryDb");
            //    return new NineskyDbContext(optionsBuilder.Options);
            //}, Lifestyle.Scoped);

            //_container.Register<SimpleInjectorActivatedMiddleware>();

            //_container.Verify();

            #endregion

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

            //注册SingalR服务
            services.AddSignalR();

            //注册自定义筛选服务（执行顺序 中间件处理前后）
            services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();

            //注册自定义工厂中间件
            //services.AddTransient<FactoryActivatedMiddleware>();

            //注册目录浏览服务
            services.AddDirectoryBrowser();

            //注册路由中间件 必须在 Startup.Configure 方法中配置路由
            services.AddRouting();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //顺序1
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            //静态文件在wwwroot
            app.UseStaticFiles();
            //配置uEdtior编辑器
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //         Path.Combine(Directory.GetCurrentDirectory(), "upload")),
            //    RequestPath = "/upload",
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
            //    }
            //});

            //启用目录浏览（安全考虑，目录浏览默认处于禁用状态）
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });

            //压缩响应文件（注意顺序 只压缩其下面的内容）
            //app.UseResponseCompression();

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
                //添加自定义视图搜索路径
               

                //注册一个区域
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");    
                routes.MapRoute(
                   name: "default",
                   template: "{controller=BootStrap}/{action=Index}/{id?}");

                #region 路由测试
                //模板  /Blog/All-About-Routing/Introduction 
                //匹配结果：{ controller = Blog, action = ReadArticle, article = All-About-Routing/Introduction }
                //routes.MapRoute(name: "blog",
                //                template: "blog/{*article}",
                //                defaults: new { controller = "Blog", action = "Article" });

                //路由约束和数据令牌
                //模板 /en-US/Products/5 
                //{ controller = Products, action = Details, id = 5 }
                //数据令牌 { locale = en-US }
                //routes.MapRoute(
                //      name: "us_english_products",
                //      template: "en-US/Products/{id}",
                //      defaults:new { controller= "Products", action= "Details" },
                //      constraints:new { id = new IntRouteConstraint() },
                //      dataTokens:new { locale = "en-US"});            
                #endregion

                //注册AutoMapper自定义的实体关系映射
                Mappings.RegisterMapping();
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

            //app.UseConventionalMiddleware();
            //工厂中间件，先在配置服务中注册，然后再这里使用
            //app.UseFactoryActivatedMiddleware();
            #endregion

            #region 使用路由中间件 RouteBuilder（必须在配置服务中配置路由）
            var trackPackageRouterHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync($"Hello! Route values: {string.Join(", ", routeValues)}");
            });

            var routeBuilder = new RouteBuilder(app, trackPackageRouterHandler);
            routeBuilder.MapRoute(
                  name: "Track Package Route",
                  template: "package/{operation:regex(^track|create|detonate$)}/{id:int}");

            routeBuilder.MapGet("hello/{name}", context =>
           {
               var name = context.GetRouteValue("name");
               return context.Response.WriteAsync($"Hi,{name}");
           });

            var route = routeBuilder.Build();          
            app.UseRouter(route);

            #endregion

            #region URL重写中间件(后面那几个方法不对)
            //using (StreamReader apacheModRewriteStreamReader = File.OpenText("ApacheModRewrite.txt"))
            //using (StreamReader iisUrlRewriteStreamReader = File.OpenText("IISUrlRewrite.xml"))
            //{
            //    var options = new RewriteOptions()
            //        .AddRedirect("redirect-rule/(.*)", "redirected/$1")
            //        .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2", skipRemainingRules: true)
            //        .AddApacheModRewrite(apacheModRewriteStreamReader)
            //        .AddIISUrlRewrite(iisUrlRewriteStreamReader)
            //        .AddRedirectToHttps(301);//端口默认为443
            //    //.Add(MethodRules.RedirectXMLRequests)
            //    //.Add(new RedirectImageRequests(".png", "/png-images"))
            //    //.Add(new RedirectImageRequests(".jpg", "/jpg-images"));

            //    //    app.UseRewriter(options);
            //    //}

            //    #endregion

            //    //初始化数据库
            //    //Models.DbInitializer.Initialize();

            //    //app.Run(async context => {
            //    //    await context.Response.WriteAsync("Hello,World!");
            //    //});
            //}
            #endregion
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
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used={branchVer}");
            });
        }
        #endregion
    }
}
