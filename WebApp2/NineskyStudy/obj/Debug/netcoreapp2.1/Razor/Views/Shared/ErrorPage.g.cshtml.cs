#pragma checksum "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\Shared\ErrorPage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5e3280eef933bde02032963d0d60d557abd0d680"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_ErrorPage), @"mvc.1.0.view", @"/Views/Shared/ErrorPage.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/ErrorPage.cshtml", typeof(AspNetCore.Views_Shared_ErrorPage))]
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
#line 1 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\_ViewImports.cshtml"
using NineskyStudy;

#line default
#line hidden
#line 2 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\_ViewImports.cshtml"
using NineskyStudy.Models;

#line default
#line hidden
#line 3 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\_ViewImports.cshtml"
using NineskyStudy.Base;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e3280eef933bde02032963d0d60d557abd0d680", @"/Views/Shared/ErrorPage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6fcc6b30fb93ad7510811980f68ee5fd71a4b2f6", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_ErrorPage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ErrorModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\Shared\ErrorPage.cshtml"
  
    ViewData["Title"] = Model.Title;

#line default
#line hidden
            BeginContext(64, 26, true);
            WriteLiteral("\r\n<h2 class=\"text-danger\">");
            EndContext();
            BeginContext(91, 10, false);
#line 6 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\Shared\ErrorPage.cshtml"
                   Write(Model.Name);

#line default
#line hidden
            EndContext();
            BeginContext(101, 16, true);
            WriteLiteral("</h2>\r\n<p>\r\n    ");
            EndContext();
            BeginContext(118, 27, false);
#line 8 "E:\GitSourceCode\NetCoreSample\WebApp\NineskyStudy\Views\Shared\ErrorPage.cshtml"
Write(Html.Raw(Model.Description));

#line default
#line hidden
            EndContext();
            BeginContext(145, 10, true);
            WriteLiteral("\r\n</p>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ErrorModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
