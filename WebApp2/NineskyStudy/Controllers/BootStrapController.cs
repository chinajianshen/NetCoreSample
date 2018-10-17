using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NineskyStudy.Models;
using NineskyStudy.InterfaceBase;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NineskyStudy.Infrastructure;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.Extensions.Logging;


namespace NineskyStudy.Controllers
{
    public class BootStrapController : Controller
    {
        private readonly InterfaceCategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IOptions<MyOptions> _optionAccessor;
        private readonly IOptions<MyOptionsWithDelegateConfig> _optionsAccessorWithDelegate;
        private readonly ILogger _logger;
      

        public BootStrapController(InterfaceCategoryService categoryService,IMapper mapper, IOptions<MyOptions> optionAccessor
                , IOptions<MyOptionsWithDelegateConfig> optionsAccessorWithDelegate, ILogger<BootStrapController> logger) // ILoggerFactory logger
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _optionAccessor = optionAccessor;
            _optionsAccessorWithDelegate = optionsAccessorWithDelegate;
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
             _logger = logger;
           //_logger = logger.CreateLogger("NineskyStudy.Controllers.BootStrapController"); //通过工厂这种方法不方便
        }
        public IActionResult Index()
        {
            using (_logger.BeginScope("Message attached to logs created in the using block")) {
                _logger.LogInformation(LoggingEvents.GetItem, "Getting item {ID}", 111);
                HttpContext.Session.SetString("SessionStartedTime", "Session started time:" + DateTime.Now.ToString());
                HttpContext.Session.Set<CategoryViewModel>("CateModel", new CategoryViewModel { Description = "123", Name = "张三" });
                MyOptions myOptions = _optionAccessor.Value;
                MyOptionsWithDelegateConfig myoptions2 = _optionsAccessorWithDelegate.Value;
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetById({ID}) NOT FOUND", 111);

                _logger.IndexPageRequested();
                _logger.QuoteAdded("quote123");
            }
              
            return View();
        }

        public IActionResult Grid()
        {
            var model = HttpContext.Session.Get<CategoryViewModel>("CateModel");
            var time = HttpContext.Session.GetString("SessionStartedTime");

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