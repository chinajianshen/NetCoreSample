using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NineskyStudy.Models;
using NineskyStudy.InterfaceBase;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace NineskyStudy.Controllers
{
    public class BootStrapController : Controller
    {
        private readonly InterfaceCategoryService _categoryService;
        private readonly IMapper _mapper;

        public BootStrapController(InterfaceCategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {            
            HttpContext.Session.SetString("SessionStartedTime", "Session started time:" + DateTime.Now.ToString());
            return View();
        }

        public IActionResult Grid()
        {
            return View();
        }

        public IActionResult Table()
        {
            List<Category> categoryList = _categoryService.FindList().ToList();
            
            var categoryViewModel = Mapper.Map<Category>(categoryList.First());
            var categoryViewModel2 = Mapper.Map<Category, CategoryViewModel>(categoryList.First());
            var categoryViewModel3 = Mapper.Map<CategoryViewModel>(categoryList.First());
            return View(categoryList);
        }
    }
}