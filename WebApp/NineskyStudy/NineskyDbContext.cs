/*
   添加数据上下文
   https://www.cnblogs.com/mzwhj/p/6147900.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NineskyStudy.Base;

namespace NineskyStudy
{
    public class NineskyDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public NineskyDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
