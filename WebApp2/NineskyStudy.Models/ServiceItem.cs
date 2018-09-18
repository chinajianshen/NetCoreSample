using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.Models
{

    /// <summary>
    /// 注入服务配置
    /// </summary>
    public class ServiceItem
    {
        /// <summary>
        /// 服务类型[含命名空间]
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 实现类类型[含命名空间]
        /// </summary>
        public string ImplementationType { get; set; }

        /// <summary>
        /// 生命周期
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceLifetime LifeTime { get; set; }
    }

}
