using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using SwaggerCustom.Test.Filters;
using SwaggerCustom.Test.Models;
using Swashbuckle.Swagger.Annotations;

namespace SwaggerCustom.Test.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [SwaggerControllerGroup("用户", "管理")]
    [RoutePrefix("api/user/user")]

    public class UserController : ApiController
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [Route("GetUser")]
        public string GetUser(int userId)
        {
            return "get user success";
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("PostUser")]
        public string PostUser(Models.User user)
        {
            return "post user success";
        }

        /// <summary>
        /// 批量新增用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Route("PostUsers")]
        public IHttpActionResult PostUsers(List<Models.User> users)
        {
           // System.Collections.Generic.HashSet<string>
            List<Models.User> userList = new List<Models.User>();
            userList.Add(new Models.User { Name="张三", UserId=1 });
            userList.Add(new Models.User { Name = "张四", UserId = 2 });
            return Ok(userList);
        }
    }
    }
