using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp2.Controllers
{
    [Route("about")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("phone")]
        public string Phone() {
            return "+10086";
        }

        [Route("country")]
        public string Country()
        {
            return "中国";
        }
    }
}