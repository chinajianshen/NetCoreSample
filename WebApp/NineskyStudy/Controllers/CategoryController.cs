using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NineskyStudy.Base;

namespace NineskyStudy.Controllers
{
    public class CategoryController : Controller
    {
        /// <summary>
        ///  数据上下文
        /// </summary>
        private readonly NineskyDbContext _dbContext;

        /// <summary>
        /// 栏目服务
        /// </summary>
        private readonly CategoryService _categoryService;

        public CategoryController(NineskyDbContext dbContext)
        {
            _dbContext = dbContext;
            _categoryService = new CategoryService(dbContext);

        }

        /// <summary>
        /// 查看栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Category/{id:int}")]
        public IActionResult Index(int id)
        {
            var category = _categoryService.Find(id);
            if (category == null) return View("errorpage", new Models.ErrorModel { Title = "错误消息", Name = "栏目不存在", Description = "访问ID为【" + id + "】的栏目时发生错误，该栏目不存在。" });
            switch (category.Type)
            {
                case CategoryType.General:
                    if (category.General == null) return View("Error", new Models.ErrorModel { Title = "错误消息", Name = "栏目数据不完整", Description = "找不到栏目【" + category.Name + "】的详细数据。" });
                    return View(category.General.View, category);
                case CategoryType.Page:
                    if (category.Page == null) return View("Error", new Models.ErrorModel { Title = "错误消息", Name = "栏目数据不完整", Description = "找不到栏目【" + category.Name + "】的详细数据。" });
                    return View(category.Page.View, category);
                case CategoryType.Link:
                    if (category.Link == null) return View("Error", new Models.ErrorModel { Title = "错误消息", Name = "栏目数据不完整", Description = "找不到栏目【" + category.Name + "】的详细数据。" });
                    return Redirect(category.Link.Url);
                default:
                    return View("Error", new Models.ErrorModel { Title = "错误消息", Name = "栏目数据错误", Description = "栏目【" + category.Name + "】的类型错误。" });

            }            
        }

        public IActionResult Page()
        {
            return View();
        }
    }
}