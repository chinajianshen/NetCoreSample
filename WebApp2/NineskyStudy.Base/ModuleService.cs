using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using NineskyStudy.InterfaceBase;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NineskyStudy.Base
{
    public class ModuleService : BaseService<Module>, InterfaceModuleService
    {
        public ModuleService(DbContext dbContext) : base(dbContext)
        {

        }

        public override Module Find(int Id)
        {
            return _dbContext.Set<Module>().Include(m => m.ModuleOrders).SingleOrDefault(m => m.ModuleId == Id);
        }

        public async Task<IQueryable<Module>> FindListAsync(bool? enable)
        {
            if (enable == null) return await base.FindListAsync();
            else return await base.FindListAsync(m => m.Enabled == enable);
        }

        public async Task<IQueryable<ModuleOrder>> FindOrderListAsync(int moduledId)
        {
            return await Task.FromResult(_dbContext.Set<ModuleOrder>().Where(mo => mo.ModuleId == moduledId));
        }
    }
}
