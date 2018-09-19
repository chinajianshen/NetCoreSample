using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;

namespace NineskyStudy.Areas.System.Controllers
{
    [Area("System")]
    public class ModuleController : Controller
    {
        private readonly InterfaceModuleService _interfaceModuleService;

        public ModuleController(InterfaceModuleService interfaceModuleService)
        {
            _interfaceModuleService = interfaceModuleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 详细
        /// </summary>
        /// <param name="id">模块ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id)
        {
            return View(await _interfaceModuleService.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Enable(int id,bool enabled)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var module = await _interfaceModuleService.FindAsync(id);
            if (module == null)
            {
                jsonResponse.succeed = false;
                jsonResponse.message = "模块不存在";
            }
            else
            {
                module.Enabled = enabled;
                if (await _interfaceModuleService.UpdateAsync(module))
                {
                    jsonResponse.succeed = true;
                    jsonResponse.message = $"模块已{(enabled ? "启用":"禁用")}";
                }
                else
                {
                    jsonResponse.succeed = false;
                    jsonResponse.message = "保存数据失败";
                }
            }
            return Json(jsonResponse);
        }

        /// <summary>
        /// 模块列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> List()
        {
            return Json((await _interfaceModuleService.FindListAsync()).ToList());
        }

        /// <summary>
        /// 排序列表
        /// </summary>
        /// <param name="id">模块Id</param>
        /// <returns></returns>
        public async Task<IActionResult> OrderList(int id)
        {
            var moduleOrderList = await _interfaceModuleService.FindOrderListAsync(id);
            return Json(moduleOrderList);
        }

    }
}