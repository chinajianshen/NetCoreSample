using Microsoft.AspNetCore.Mvc;
using NineskyStudy.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Components
{
    public class CategoryNavViewComponent: ViewComponent
    {
        private readonly InterfaceCategoryService _categoryService;

        public CategoryNavViewComponent(InterfaceCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //可以获取上下信息
            //var id = RouteData.Values["id"] as string;
             var categories = await _categoryService.FindChildrenAsync(0);
            return View(categories);
        }
    }
}
