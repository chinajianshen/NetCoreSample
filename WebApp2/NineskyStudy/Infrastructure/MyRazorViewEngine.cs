using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    //ASP.NET Core框架实现了组件化，很多功能都通过IoC的方式修改或扩展，传统ASP.NET MVC继承RazorViewEngine 已经不行了
    //public class MyRazorViewEngine: RazorViewEngine
    //{
    //public MyRazorViewEngine(IRazorPageFactory pageFactory,
    // IRazorViewFactory viewFactory,
    // IViewLocationExpanderProvider viewLocationExpanderProvider,
    // IViewLocationCache viewLocationCache)
    // : base(pageFactory,
    //       viewFactory,
    //       viewLocationExpanderProvider,
    //       viewLocationCache)
    //{
    //}

    //public override IEnumerable<string> AreaViewLocationFormats
    //{
    //    get
    //    {
    //        var value = newRandom().Next(0, 1);
    //        var theme = value == 0 ? "Theme1" : "Theme2";
    //        returnbase.AreaViewLocationFormats.Select(f => f.Replace("/Views/", "/Views/" + theme + "/"));
    //    }
    //}

    //public override IEnumerable<string> ViewLocationFormats
    //{
    //    get
    //    {
    //        var value = newRandom().Next(0, 1);
    //        var theme = value == 0 ? "Theme1" : "Theme2";
    //        returnbase.ViewLocationFormats.Select(f => f.Replace("/Views/", "/Views/" + theme + "/"));
    //    }
    //}
    //}

    /// <summary>
    /// 实现IViewLocationExpander接口来达到扩展配置自定义视图位置
    /// </summary>
    public class TemplateViewLocationExPander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //var template = context.Values["Template"] ?? "Default";

            string[] locations = { "~/Views/{1}/Template/{0}.cshtml" };
            return locations.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //context.Values["Template"] = context.ActionContext.RouteData.Values["Template"]?.ToString() ?? "Default";
        }
    }

    public class TemplateAreaViewLocationExPander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (!string.IsNullOrEmpty(context.Values["Template"]))
            {
                string[] locations = { "~/Areas/{2}/Views/{1}/Template/{0}.cshtml" };
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["template"] = context.ActionContext.RouteData.Values["Template"]?.ToString() ?? "";
        }
    }

}
