using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BLL;
using Server.Filters;
using Server.Helper;
using WebApiContract.Models;
using System.Text.RegularExpressions;
using CommLib;

namespace Server.Controllers
{
    [WebApiAuthFilter]
    public class UsersInfoController : ApiController
    {
        // GET api/usersinfo
        // 获取用户列表，只有管理员有权限执行
        [WebApiRoleFilter(RoleType.ADMINISTARTOR)]
        public IEnumerable<UserInfo_API_Get> Get()
        {
            return Mapper.Map<IEnumerable<UserInfo_API_Get>>(Managers.s_userManager.GetUsers());
        }

        // GET api/usersinfo/{username}
        // 获取用户信息，非管理员只能获取自己的
        public UserInfo_API_Get Get(string id)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);
            UserInfo_BLL userinfo = Managers.s_userManager.GetUser(id);
            return Mapper.Map<UserInfo_API_Get>(userinfo);
        }

        // POST api/usersinfo
        // 添加一个用户，只有管理员有权执行
        [WebApiRoleFilter(RoleType.ADMINISTARTOR)]
        public HttpResponseMessage Post(UserInfo_API_Post userinfo)
        {
            throw new NotImplementedException();
        }

        // PUT api/usersinfo/{username}
        // 更新用户信息，非管理员只能更新自己的
        public void Put(string id, UserInfo_API_Put userinfo, long updateticks)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);

            //非管理员忽略掉Role的修改
            if (this.GetUserRole()!=RoleType.ADMINISTARTOR)
            {
                userinfo.Role = null;
            }

            UserInfo_BLL ui_bll = Mapper.Map<UserInfo_BLL>(userinfo);
            ui_bll.UserName = id;
            ui_bll.UpdateTicks = updateticks;
            Managers.s_userManager.SetUser(ui_bll);
        }

        // DELETE api/usersinfo/{username}
        // 删除用户，只有管理员有权执行
        [WebApiRoleFilter(RoleType.ADMINISTARTOR)]
        public void Delete(string id, long updateticks)
        {
            throw new NotImplementedException();
        }
    }
}
