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
    /// 管理员角色
    /// </summary>
    [SwaggerControllerGroup("Admin", "Role")]
    [RoutePrefix("api/admin/role")]
    public class RoleController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        [Route("GetAdminRoles")]
        [HttpGet]
        public string GetAdminRoles()
        {
            return "角色返回";
        }
    }
}
