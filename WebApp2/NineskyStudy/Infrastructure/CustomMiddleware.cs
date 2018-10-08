using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    /// <summary>
    /// 按请求依赖项
    /// </summary>
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IMyScopedService svc)
        {
            svc.MyProperty = 1000;
            await _next(httpContext);
        }
    }

    public class MyScopedService : IMyScopedService
    {
        public int MyProperty { get; set; }
    }

    public interface IMyScopedService
    {
        int MyProperty { get; set; }
    }
}
