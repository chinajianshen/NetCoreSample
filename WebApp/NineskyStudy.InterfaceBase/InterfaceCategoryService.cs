using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.InterfaceBase
{
   public interface InterfaceCategoryService:InterfaceBaseService<Category>
    {
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        Category Find(int id);
    }
}
