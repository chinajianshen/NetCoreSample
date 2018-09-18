using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp2.Models
{
    public class HelloWorldDBContext: IdentityDbContext<User>//DbContext
    {
        public HelloWorldDBContext() { }

        //Starup中第一种注册服务写法
        //public HelloWorldDBContext(DbContextOptions<HelloWorldDBContext> options) : base(options)
        //{

        //}

        //Starup中第一种注册服务写法
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
