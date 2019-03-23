using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueStudy.Controllers
{
    public class VueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VueIf()
        {
            return View();
        }

        public IActionResult VueCircle()
        {
            return View();
        }

        public IActionResult VueComputed()
        {
            return View();
        }

        public IActionResult VueWatch()
        {
            return View();
        }

        public IActionResult VueStyle()
        {
            return View();
        }

        public IActionResult VueEvent()
        {
            return View();
        }

        public IActionResult VueForm()
        {
            return View();
        }

        public IActionResult VueComponent()
        {
            return View();
        }

        public IActionResult VueCustomDirective()
        {
            return View();
        }

        public IActionResult VueRouting()
        {
            return View();
        }

        public IActionResult VueTransition()
        {
            return View();
        }

        public IActionResult VueMinin()
        {
            return View();
        }

        public IActionResult VueAjax()
        {
            return View();
        }

        public IActionResult GetData(int prm1,string prm2)
        {
            //int.Parse(prm2);
            return Json(new { state = prm1, msg = prm2 });
        }

        [HttpPost]
        public IActionResult PostData(int prm1, string prm2)
        {
            return Json(new { state = prm1, msg = prm2 });
        }

        public IActionResult VueReactive()
        {
            return View();
        }
    }
}