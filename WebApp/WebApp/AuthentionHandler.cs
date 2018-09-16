using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp
{
    /// <summary>
    /// ASP.NET Core 认证系统
    /// 定义一个认证Handler
    /// </summary>
    public class MyHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies["myCookie"];
            if (string.IsNullOrEmpty(cookie))
            {
               // return AuthenticateResult.NoResult();
            }
            // return AuthenticateResult.Success();
            return Task.FromResult<AuthenticateResult>(null);
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }



        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            // Context.Response.Cookies.Append("myCookie", )
            // TicketDataFormat dataFormat = new TicketDataFormat(ticket);
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete("myCookie");
            return Task.CompletedTask;
        }
    }

    public class TestHandler
    {
        private void Test()
        {
            //ClaimsIdentity

        }

        private void CreateIdentity()
        {
            //声明身份
            //创建一个用户身份，注意需要指定AuthenticationType，否则IsAuthenticated将为false。
            var claimIndentity = new ClaimsIdentity("myAuthenticationType");
            claimIndentity.AddClaim(new Claim(ClaimTypes.Name, "bob"));
            claimIndentity.AddClaim(new Claim(ClaimTypes.Email, "bob@qq.com"));
            claimIndentity.AddClaim(new Claim(ClaimTypes.MobilePhone, "15300255979"));

            //声明负责人 （包含多种身份）
            //ClaimsPrincipal 
            var principal = new ClaimsPrincipal(claimIndentity);

            //将principal包装成 AuthenticationTicket（认证票据）
            var properties = new AuthenticationProperties();
            properties.ExpiresUtc = new DateTimeOffset().AddDays(7);
            var ticket = new AuthenticationTicket(principal, properties, "myScheme");
            //scheme 方案
            //
            // 加密 序列化 得到令牌 票据
            //var token = Protect(ticket);
        }
    }
}
