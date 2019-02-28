using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace SwaggerCustom.Test.Filters
{
    public class GlobalHttpHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            ////var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline(); //判断是否添加权限过滤器
            ////var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is IAuthorizationFilter); //判断是否允许匿名方法 

            //var isNeedLogin = apiDescription.ActionDescriptor.GetCustomAttributes<CustomActionFilterAttribute>().Any(); //是否有验证用户标记
            //if (isNeedLogin)//如果有验证标记则 多输出2个文本框(swagger form提交时会将这2个值放入header里)
            //{               
            //    operation.parameters.Add(new Parameter { name = "t8_token", @in = "header", description = "token", required = true, type = "string" });
            //    operation.parameters.Add(new Parameter { name = "t8_tick", @in = "header", description = "tick", required = true, type = "string" });
            //}

            operation.parameters.Add(new Parameter { name = "t8_token", @in = "header", description = "token", required = true, type = "string" });
            operation.parameters.Add(new Parameter { name = "t8_tick", @in = "header", description = "tick", required = true, type = "string" });
        }
    }
}