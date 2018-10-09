using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NineskyStudy.Infrastructure;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;

namespace NineskyStudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly InterfaceModuleService _moduleService;
        private readonly InterfaceCategoryService _categoryService;
        private readonly IOptions<AppOptions> _options;


        public HomeController(InterfaceModuleService moduleService,InterfaceCategoryService categoryService, IOptions<AppOptions> options)
        {
            _moduleService = moduleService;
            _categoryService = categoryService;
            _options = options;
        }

        public IActionResult Index()
        {
            var routeData = ControllerContext.RouteData;
            //数据库初始化
            DbInitializer.InitializeModule(_moduleService);
            DbInitializer.InitializeCategory(_categoryService);

            return View();
        }

        public IActionResult T_List()
        {
            return View();
        }

        private void InitializeModule()
        {
           if ( _moduleService.FindList().Count()>0)
            {
                return;
            }

            var modules = new List<Module>();
            var module = new Module()
            {
                Controller = "Article",
                Description = "实现文章功能",
                Enabled = true,
                Name = "文章模块",
                ModuleOrders = new List<ModuleOrder> {
                new ModuleOrder {   Name="ID升序", Order=0},
                new ModuleOrder { Name="ID降序",Order=1 },
                new ModuleOrder {Name="发布时间升序", Order=2 },
                new ModuleOrder { Name="发布时间降序",Order=3},
                new ModuleOrder { Name="点击升序",Order=4},
                new ModuleOrder { Name="点击降序",Order=5}
            }
            };
            modules.Add(module);
            _moduleService.AddRange(modules.ToArray());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
