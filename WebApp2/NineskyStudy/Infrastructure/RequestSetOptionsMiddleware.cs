using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    public class RequestSetOptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private IOptions<AppOptions> _injectedOptions;

        public RequestSetOptionsMiddleware(RequestDelegate next,IOptions<AppOptions> injectedOptions)
        {
            _next = next;
            _injectedOptions = injectedOptions;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var option = httpContext.Request.Query["option"];

            if (!string.IsNullOrWhiteSpace(option))
            {                
                _injectedOptions.Value.Option = WebUtility.HtmlEncode(option);
            }

            await _next(httpContext);
        }

    }

    public class AppOptions
    {
        public string Option { get; set; }
    }
}
