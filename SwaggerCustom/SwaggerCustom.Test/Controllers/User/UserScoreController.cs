using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace SwaggerCustom.Test.Controllers.User
{
    /// <summary>
    /// 用户积分管理
    /// </summary>
    [SwaggerControllerGroup("用户", "积分管理")]
    [RoutePrefix("api/user/score")]

    public class UserScoreController : ApiController
    {
        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <returns></returns>
        [Route("GetUserScore")]
        public Models.User GetUserScore(int userId)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            return new Models.User() { UserId = userId, Name = "ben" };
        }
    }
}
