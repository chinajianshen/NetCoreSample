using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NineskyStudy.Base;
using NineskyStudy.DataLibrary;
using NineskyStudy.InterfaceBase;
using NineskyStudy.InterfaceDataLibrary;
using NineskyStudy.Models;

namespace NineskyStudy
{
    public class Startup
    {
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration,IHostingEnvironment env)
        {
            Configuration = configuration;

            //ContentRootPath  用于包含应用程序文件如C:\MyApp\
            //WebRootPath 用于包含Web服务性的内容文件C:\MyApp\wwwroot\
            _contentRootPath = env.ContentRootPath;
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
                conn = conn.Replace("%CONTENTROOTPATH%",_contentRootPath);
            }

            //注入NineskyDbContext 参照 https://www.cnblogs.com/mzwhj/p/6147900.html 第4部分
            services.AddDbContext<NineskyDbContext>(options => {
                options.UseSqlServer(conn);                
            });
            //数据库与接口注入
            services.AddScoped<DbContext, NineskyDbContext>();          
            services.AddScoped<InterfaceBaseRepository<Category>, BaseRepository<Category>>();

            //自己添加接口注入
            services.AddScoped<InterfaceBaseService<Category>, BaseService<Category>>();
            services.AddScoped<InterfaceCategoryService, CategoryService>();

            //services.AddScoped<CategoryService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
                //注册一个区域
                routes.MapRoute(
                    name:"area",
                    template:"{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
