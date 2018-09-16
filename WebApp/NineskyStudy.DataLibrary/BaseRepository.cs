using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.DataLibrary
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public class BaseRepository<T> where T:class
    {
        private readonly DbContext _dbContext;
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
    }
}
