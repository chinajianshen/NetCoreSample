/*
  参考地址:https://www.cnblogs.com/mzwhj/p/6147900.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NineskyStudy.InterfaceBase;

using NineskyStudy.Models;

namespace NineskyStudy.Base
{
    /// <summary>
    /// 栏目服务类
    /// </summary>
    public class CategoryService : BaseService<Category>, InterfaceCategoryService
    {
        public CategoryService(DbContext dbContext):base(dbContext)
        {

        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="Id">栏目Id</param>
        /// <returns></returns>
        public override Category Find(int Id)
        {
            return _dbContext.Set<Category>()
                   .Include("General")
                   .Include("Page")
                   .Include("Link")
                   .SingleOrDefault(c => c.CategoryId == Id);
        }

        public List<Category> FindTree(CategoryType? categoryType)
        {
            var categories = _dbContext.Set<Category>().AsQueryable();


            //根据栏目类型分类处理
            switch (categoryType)
            {
                case null:
                    break;
                case CategoryType.General:
                    categories = categories.Where(c => c.Type == categoryType);
                    break;
                default:
                    //默认-Page或Link类型
                    //Id数组-含本栏目及父栏目
                    List<int> idArray = new List<int>();
                    //查找栏目id及父栏目路径
                    var categoryArray = categories.Where(c => c.Type == categoryType).Select(c => new { CategoryId = c.CategoryId, ParentPath = c.ParentPath });
                    if (categoryArray != null)
                    {
                        //添加栏目ID到
                        idArray.AddRange(categoryArray.Select(c => c.CategoryId));
                        foreach (var parentPath in categoryArray.Select(c => c.ParentPath))
                        {
                            var parentIdArray = parentPath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parentIdArray != null)
                            {
                                int parseId = 0;
                                foreach (var parentId in parentIdArray)
                                {
                                    if (int.TryParse(parentId, out parseId)) idArray.Add(parseId);
                                }
                            }
                        }
                    }
                    categories = categories.Where(c => idArray.Contains(c.CategoryId));
                    break;
            }
            return categories.OrderBy(c => c.ParentPath).ThenBy(c => c.Order).ToList();
        }
    }
}
