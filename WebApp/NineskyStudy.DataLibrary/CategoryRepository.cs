using Microsoft.EntityFrameworkCore;
using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NineskyStudy.DataLibrary
{
    /// <summary>
    /// 栏目仓储
    /// </summary>
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public override Category Find(Expression<Func<Category, bool>> predicate)
        {
            return _dbContext.Set<Category>()
                .Include(c => c.General)
                .Include(c => c.Page)
                .Include(c => c.Link)
                .SingleOrDefault(predicate);
        }

    }
}
