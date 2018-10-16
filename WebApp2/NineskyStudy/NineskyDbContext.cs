/*
   添加数据上下文
   https://www.cnblogs.com/mzwhj/p/6147900.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NineskyStudy.Models;

namespace NineskyStudy
{
    public class NineskyDbContext:DbContext
    {
        /// <summary>
        /// 模块
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        public DbSet<EFConfigurationValue> Values { get; set; }

        public NineskyDbContext(DbContextOptions options) : base(options)
        {

        }
    }

    public class EFConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        /// <summary>
        /// 将自定义配置提供程序扩展到IConfigurationProvider
        /// </summary>
        /// <param name="optionsAction"></param>
        public EFConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigurationProvider(_optionsAction);
        }
    }

    /// <summary>
    ///  自定义配置提供程序
    /// </summary>
    public class EFConfigurationProvider : ConfigurationProvider
    {
        Action<DbContextOptionsBuilder> OptionsAction { get; }
        public EFConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<NineskyDbContext>();
            OptionsAction(builder);

            using (var dbContext = new NineskyDbContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();

                Data = !dbContext.Values.Any() ? CreateAndSaveDefaultValues(dbContext) : dbContext.Values.ToDictionary(c => c.Id, c => c.Value);
            }               
        }

        private static IDictionary<string,string> CreateAndSaveDefaultValues(NineskyDbContext dbContext)
        {
            var configValues = new Dictionary<string, string> {
                 { "quote1", "I aim to misbehave." },
                { "quote2", "I swallowed a bug." },
                { "quote3", "You can't stop the signal, Mal." }
            };

            dbContext.Values.AddRange(configValues.Select(kvp => new EFConfigurationValue
            {
                Id = kvp.Key,
                Value = kvp.Value
            }).ToArray());

            dbContext.SaveChanges();
            return configValues;
        }
    }
}
