using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;

namespace NineskyStudy.Areas.System.Controllers
{
     [Area("System")]
    public class CategoryController : Controller
    {
        private InterfaceCategoryService _categoryService;
        public CategoryController(InterfaceCategoryService categoryService)
        {
            _categoryService = categoryService;
        }
      
        public async Task<IActionResult> Add([FromServices]InterfaceModuleService moduleService, CategoryType? categoryType)
        {
            var modules = await moduleService.FindListAsync(true);
            var moduleArray = modules.Select(m => new SelectListItem { Text = m.Name,Value=m.ModuleId.ToString() }).ToList();
            moduleArray.Insert(0, new SelectListItem { Text = "无", Value = "0", Selected = true });
            ViewData["Modules"] = moduleArray;
            return View(new Category() { Type = CategoryType.General, ParentId = 0, View = "Index", Order = 0, Target = LinkTarget._self, General = new CategoryGeneral() { ContentView = "Index" } });

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}