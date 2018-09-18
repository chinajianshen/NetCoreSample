using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundatio.Caching;
using Microsoft.AspNetCore.Mvc;

namespace WebApp2.Controllers
{
    public class FoundatioController : Controller
    {
        public  IActionResult Index()
        {
            
            return View();
        }

        private async Task<CacheValue<string>> GetCache(string cachekey,string cachevalue)
        {
            ICacheClient cache = new InMemoryCacheClient();
            await cache.SetAsync(cachekey, cachevalue);
            return await cache.GetAsync<string>(cachekey);           
        }
    }
}