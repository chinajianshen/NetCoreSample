using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace NineskyStudy.Areas.System.Controllers
{
     [Area("System")]
    public class CategoryController : Controller
    {
           

        public CategoryController()
        {
            
        }

        public IActionResult Index()
        {
            return Content("11111");
        }
    }
}