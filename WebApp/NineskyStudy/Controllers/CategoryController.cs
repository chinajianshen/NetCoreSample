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
            if (category == null) return View("error");
            return View(category);
        }
    }
}