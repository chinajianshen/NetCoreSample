using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.InterfaceBase
{
    public interface InterfaceCategoryService:InterfaceBaseService<Category>
    {
        /// <summary>
        /// 查找树形菜单
        /// </summary>
        /// <param name="categoryType">栏目类型，可以为空</param>
        /// <returns></returns>
        List<Category> FindTree(CategoryType? categoryType);
    }
}
