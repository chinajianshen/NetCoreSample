using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LoggingEvents
    {
        public const int GET_ITEM = 1002;
        public const int GET_ITEM_NOTFOUND = 4000;
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyOptions _options;

        public HomeController(ILogger<HomeController> logger, IOptions<MyOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("你访问了首页");
            _logger.LogWarning("警告信息");
            _logger.LogError("错误信息");

            _logger.LogInformation(LoggingEvents.GET_ITEM, "Getting item {ID}.{time}", 5,DateTime.Now.ToString());
            
            string a1 = _options.DefaultValue;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            //AuthenticationHttpContextExtensions
            //IAuthenticationService
            //IClaimsTransformation
            //IAuthenticationHandlerProvider

            //IAuthenticationSchemeProvider //scheme供应者接口
            // AuthenticationSchemeProvider //实现
            //AuthenticationScheme
            //AuthenticationOptions

            //IAuthenticationHandler
            //IAuthenticationHandlerProvider
            //AuthenticationHandlerProvider

            //IAuthenticationService对IAuthenticationSchemeProvider和IAuthenticationHandlerProvider封装
            //AuthenticationService

            //IAuthenticationHandler IAuthenticationSignInHandler IAuthenticationSignOutHandler
            
            return null;
        }
    }
}
