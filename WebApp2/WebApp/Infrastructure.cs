using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp
{
    public class Infrastructure
    {
        
        public string Name { get; set; } = "summit";

        public int Age { get; set; } = 22;

        public IList<int> AgeList { get; set; } = new List<int> { 10, 20, 30 };

        private string Country { get; }

        public Infrastructure()
        {
            this.Country = "中" + "国";
        }
    }

    public class FloorOneMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FloorOneMiddleware> _logger;
        public FloorOneMiddleware(RequestDelegate next,ILogger<FloorOneMiddleware> logger)
        {
            _next = next;
            _logger = logger;          
        }

        public async Task InvokeAsync(HttpContext context)
        {
             _logger.LogInformation("FloorOneMiddleware In");
            //Do Something
            //To FloorTwoMiddleware
            await _next(context);
            //Do Something
            _logger.LogInformation("FloorOneMiddleware Out");

        }
    }

    public static class FloorOneMiddlewareExtensions
    {        
        public static IApplicationBuilder UseFloorOne(this IApplicationBuilder builder)
        {          
            return builder.UseMiddleware<FloorOneMiddleware>();
        }
    }

    public class TestStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(async (context, next1) =>
                {
                    Console.WriteLine("filter.Use1.begin");
                    await next1.Invoke();
                    Console.WriteLine("filter.Use1.end");
                });
                next(app);
            };
        }
    }

    internal class TokenRefreshService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public TokenRefreshService(ILogger<TokenRefreshService> logger)
        {
            _logger = logger;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void Refresh(object state)
        {
            _logger.LogInformation(DateTime.Now.ToLongTimeString() + ": Refresh Token!"); //在此写需要执行的任务
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service starting");
            _timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
    

    internal class TokenRefreshBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
        public TokenRefreshBackgroundService(ILogger<TokenRefreshBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TokenRefreshBackgroundService 开始");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(DateTime.Now.ToLongTimeString() + ": Refresh Token BackgroundService !");//在此写需要执行的任务
                await Task.Delay(5000, stoppingToken);
            }
            _logger.LogInformation("TokenRefreshBackgroundService 结束");

        }
    }

}
