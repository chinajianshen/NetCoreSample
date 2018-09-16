/*
  参考地址:https://www.cnblogs.com/mzwhj/p/6147900.html
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NineskyStudy.DataLibrary;

namespace NineskyStudy.Base
{
    /// <summary>
    /// 栏目服务类
    /// </summary>
    public class CategoryService
    {
        private BaseRepository<Category> _baseRepository;
        public CategoryService(DbContext dbContext)
        {
            _baseRepository = new BaseRepository<Category>(dbContext);
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category Find(int id)
        {
            return _baseRepository.Find(id);
        }
    }
}
