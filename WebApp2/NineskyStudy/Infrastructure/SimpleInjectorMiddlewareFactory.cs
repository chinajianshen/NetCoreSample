using Microsoft.AspNetCore.Http;
using NineskyStudy.InterfaceBase;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    /// <summary>
    /// 第三方容器中基于工厂的中间件
    /// </summary>
    public class SimpleInjectorMiddlewareFactory: IMiddlewareFactory
    {
        private readonly Container _container;

        public SimpleInjectorMiddlewareFactory(Container container)
        {
            _container = container;
        }

        public IMiddleware Create(Type middlewareType)
        {
            return _container.GetInstance(middlewareType) as IMiddleware;
        }

        public void Release(IMiddleware middleware)
        {
           // throw new NotImplementedException();
        }
    }

    public class SimpleInjectorActivatedMiddleware : IMiddleware
    {
        private readonly InterfaceCategoryService _db;

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
