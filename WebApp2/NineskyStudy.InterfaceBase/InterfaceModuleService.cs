using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineskyStudy.InterfaceBase
{
    public interface InterfaceModuleService : InterfaceBaseService<Module>
    {
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="enable">启用</param>
        /// <returns></returns>
        Task<IQueryable<Module>> FindListAsync(bool? enable);

        /// <summary>
        /// 查找排序列表
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        Task<IQueryable<ModuleOrder>> FindOrderListAsync(int moduledId);
    }
}
