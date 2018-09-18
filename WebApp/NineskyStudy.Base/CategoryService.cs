/*
  参考地址:https://www.cnblogs.com/mzwhj/p/6147900.html
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NineskyStudy.InterfaceBase;
using NineskyStudy.InterfaceDataLibrary;
using NineskyStudy.Models;

namespace NineskyStudy.Base
{
    /// <summary>
    /// 栏目服务类
    /// </summary>
    public class CategoryService:BaseService<Category>,InterfaceCategoryService
    {
        //这是刚开始写法，后面一步一步重构
        //private BaseRepository<Category> _baseRepository;
        //private CategoryRepository _categoryRepository;
        //private InterfaceBaseRepository<Category> _categoryRepository;
        //public InterfaceCategoryService _interfaceCategoryService;


        //public CategoryService(DbContext dbContext)
        //public CategoryService(InterfaceBaseRepository<Category> baseRepository)
        public CategoryService(InterfaceBaseRepository<Category> interfaceBaseRepository) :base(interfaceBaseRepository)
        {
            // _baseRepository = new BaseRepository<Category>(dbContext);
            // _categoryRepository = new CategoryRepository(dbContext);
            //_categoryRepository = new BaseRepository<Category>(dbContext);
            //_categoryRepository = baseRepository;
            //_interfaceCategoryService = interfaceCategoryService;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        public Category Find(int id)
        {
            // return _categoryRepository.Find(new string[] { "General", "Page", "Link" }, c => c.CategoryId == id);
            //return _categoryRepository.Find(id);

            //return _categoryRepository.Find(c => c.CategoryId == id);

            return base.Find(new string[] { "General", "Page", "Link" }, c => c.CategoryId == id);
        }
    }
}
