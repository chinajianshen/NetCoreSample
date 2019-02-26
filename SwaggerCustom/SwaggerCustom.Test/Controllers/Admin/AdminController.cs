using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace SwaggerCustom.Test.Controllers.Admin
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    [SwaggerControllerGroup("Admin", "Manage")]
    [RoutePrefix("api/admin/admin")]
    public class AdminController : ApiController
    {
        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns></returns>
        [Route("getalladmin")]
        public List<string> GetAllAdmin()
        {
            return new List<string>() { "ws", "wcl", "yll" };
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        [Route("PostAdmin")]
        public string PostAdmin(Model.Admin adminInfo)
        {
            return "hhe";
        }
    }
}
