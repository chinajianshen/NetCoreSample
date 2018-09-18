using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 程序集注入项目
    /// </summary>
    public class AssemblyItem
    {
        /// <summary>
        ///  服务的程序集名称[不含后缀]
        /// </summary>
        public string ServiceAssembly { get; set; }
        /// <summary>
        /// 实现程序集名称[含后缀.dll]
        /// </summary>
        public string ImplementationAssembly { get; set; }

        /// <summary>
        /// 注入服务集合
        /// </summary>
        public List<ServiceItem> DICollections { get; set; }
    }
}
