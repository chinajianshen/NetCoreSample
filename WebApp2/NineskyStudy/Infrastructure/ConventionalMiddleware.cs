using Microsoft.AspNetCore.Http;
using NineskyStudy.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    /// <summary>
    /// 使用约定激活的中间件
    /// </summary>
    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;

        public ConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsnyc(HttpContext conext, InterfaceCategoryService db)
        {
            var keyValue = conext.Request.Query["key"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                //相关逻辑
            }
            await _next(conext);
        }
    }

    /// <summary>
    /// 使用 MiddlewareFactory 激活的中间件
    /// </summary>
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private InterfaceCategoryService _db;

        public FactoryActivatedMiddleware(InterfaceCategoryService db)
        {
            _db = db;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var keyValue = context.Request.Query["key"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {

            }
            await next(context);
        }
    }
}
