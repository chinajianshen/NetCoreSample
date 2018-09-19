using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 查找树形菜单
        /// </summary>
        /// <param name="categoryType">栏目类型，可以为空</param>
        /// <returns></returns>
        Task<IQueryable<Category>> FindTreeAsync(CategoryType? categoryType);

        /// <summary>
        /// 查找子栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        IQueryable<Category> FindChildren(int id);

        /// <summary>
        /// 查找子栏目
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        Task<IQueryable<Category>> FindChildrenAsync(int id);
    }
}
