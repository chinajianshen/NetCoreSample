using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using Server.Filters;
using WebApiContract.Models;
using Server.Helper;
using AutoMapper;

namespace Server.Controllers
{
    [WebApiAuthFilter]
    public class EntranceController : ApiController
    {
        // GET api/entrance
        // 如果验证成功，就返回请求者的信息
        public UserInfo_API_Get Get()
        {
            UserInfo_BLL userinfo = Managers.s_userManager.GetUser(this.GetUserName());
            return Mapper.Map<UserInfo_API_Get>(userinfo);
        }
    }
}
