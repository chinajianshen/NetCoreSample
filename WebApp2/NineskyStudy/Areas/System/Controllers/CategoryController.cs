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

        public async Task<IActionResult> Add([FromServices]InterfaceModuleService moduleService, Category category)
        {
            if (ModelState.IsValid)
            {
                //检查父栏目
                if (category.ParentId > 0)
                {
                    var parentCategory = await _categoryService.FindAsync(category.ParentId);
                    if (parentCategory == null) ModelState.AddModelError("ParentId", "父栏目不存在");
                    else if (parentCategory.Type != CategoryType.General) ModelState.AddModelError("ParentId", "父栏目不能添加子栏目");
                    else category.ParentPath = parentCategory.ParentPath + "," + parentCategory.CategoryId;
                }
                else category.ParentPath = "0";
                //检查栏目类型
                switch (category.Type)
                {
                    case CategoryType.General:
                        if (category.General == null) ModelState.AddModelError("General.Type", "请填写常规栏目内容");
                        else
                        {
                            if (category.General.ModuleId > 0)
                            {
                                if (string.IsNullOrEmpty(category.General.ContentView)) ModelState.AddModelError("General.ContentView", "请填写栏目视图");
                                if (category.General.ContentOrder == null) ModelState.AddModelError("General.ContentOrder", "请选择内容排序方式");
                            }
                            else
                            {
                                if (category.Page != null) category.Page = null;
                                if (category.Link != null) category.Link = null;
                            }
                        }
                        break;
                    case CategoryType.Page:
                        //检查
                        if (category.Page == null) ModelState.AddModelError("General.Type", "请填写单页栏目内容");
                        else
                        {
                            if (string.IsNullOrEmpty(category.Page.Content)) ModelState.AddModelError("Page.Content", "请输入单页栏目内容");
                            else
                            {
                                if (category.General != null) category.General = null;
                                if (category.Link != null) category.Link = null;
                            }
                        }
                        break;
                    case CategoryType.Link:
                        //检查
                        if (category.Link == null) ModelState.AddModelError("General.Type", "请填写连接栏目内容");
                        else
                        {
                            if (string.IsNullOrEmpty(category.Link.Url)) ModelState.AddModelError("Link.Url", "请选择输入链接地址");
                            else
                            {
                                if (category.General != null) category.General = null;
                                if (category.General != null) category.General = null;
                            }
                        }
                        break;
                }

                //保存到数据库
                if (ModelState.IsValid)
                {
                    if (await _categoryService.AddAsync(category) > 0) return View("AddSucceed", category);
                    else ModelState.AddModelError("", "保存数据失败");
                }
            }
            var modules = await moduleService.FindListAsync(true);
            var modeleArry = modules.Select(m => new SelectListItem { Text = m.Name, Value = m.ModuleId.ToString() }).ToList();
            modeleArry.Insert(0, new SelectListItem() { Text = "无", Value = "0", Selected = true });
            ViewData["Modules"] = modeleArry;
            return View(category);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}