#pragma checksum "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\Home\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34106fbb36ef781338fd28ef11356139d278da64"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Detail), @"mvc.1.0.view", @"/Views/Home/Detail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Detail.cshtml", typeof(AspNetCore.Views_Home_Detail))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\_ViewImports.cshtml"
using WebApp2.Controllers;

#line default
#line hidden
#line 2 "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\_ViewImports.cshtml"
using WebApp2.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"34106fbb36ef781338fd28ef11356139d278da64", @"/Views/Home/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1347643869283a58b812fc2d68eae23dce8c8664", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\Home\Detail.cshtml"
  
    ViewData["Title"] = "Detail2";
   // Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(94, 156, true);
            WriteLiteral("\r\n<h1>欢迎!</h1>\r\n<div>这个消息来自 Home 控制器下的 Detail 的视图文件 Detail.cshtml</div>\r\n<p><a href=\"/\">返回首页</a></p>\r\n<table>\r\n    <tr>\r\n        <td>员工编号</td>\r\n        <td>");
            EndContext();
            BeginContext(251, 8, false);
#line 13 "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\Home\Detail.cshtml"
       Write(Model.ID);

#line default
#line hidden
            EndContext();
            BeginContext(259, 63, true);
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <td>员工姓名</td>\r\n        <td>");
            EndContext();
            BeginContext(323, 10, false);
#line 17 "C:\SourceCode\NetCore\NetCoreSample\WebApp2\WebApp2\Views\Home\Detail.cshtml"
       Write(Model.Name);

#line default
#line hidden
            EndContext();
            BeginContext(333, 59, true);
            WriteLiteral("</td>\r\n    </tr>\r\n</table>\r\n<p><a href=\"/\">返回首页</a></p>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
