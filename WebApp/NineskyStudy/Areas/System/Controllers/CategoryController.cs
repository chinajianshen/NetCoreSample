using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NineskyStudy.Base;

namespace NineskyStudy.Areas.System.Controllers
{
     [Area("System")]
    public class CategoryController : Controller
    {
        private readonly NineskyDbContext _DbContext;
        private readonly CategoryService _categoryService;

        public CategoryController(NineskyDbContext dbContext)
        {
            //_DbContext = dbContext;
            //_categoryService = new CategoryService(_DbContext);
        }

        public IActionResult Index()
        {
            return Content("11111");
        }
    }
}