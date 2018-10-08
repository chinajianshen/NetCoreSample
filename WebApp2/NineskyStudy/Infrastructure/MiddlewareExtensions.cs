using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCultrue(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }

        public static IApplicationBuilder UseCustomVal(this IApplicationBuilder builder, IMyScopedService svc)
        {
            return builder.UseMiddleware<CustomMiddleware>(svc);
        }

        public static IApplicationBuilder UseConventionalMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConventionalMiddleware>();
        }

        public static IApplicationBuilder UseFactoryActivatedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FactoryActivatedMiddleware>();
        }

        //基于 SimpleInjector第三方DI容器
        public static IApplicationBuilder UseSimpleInjectorActivatedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleInjectorActivatedMiddleware>();
        }
    }
}
