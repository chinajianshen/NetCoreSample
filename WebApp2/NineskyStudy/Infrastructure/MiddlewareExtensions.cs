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
    }
}
