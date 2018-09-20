using NineskyStudy.InterfaceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    public class DbInitializer
    {
        public static void Initialize(InterfaceModuleService moduleService)
        {
          
        }

        public static void InitializeModule(InterfaceModuleService moduleService)
        {
            if (moduleService.FindList().Count() == 0)
            {
                var modules = new List<Module>();
                var module = new Module()
                {
                    Controller = "Article",
                    Description = "实现文章功能",
                    Enabled = true,
                    Name = "文章模块",
                    ModuleOrders = new List<ModuleOrder> {
                new ModuleOrder {   Name="ID升序", Order=0},
                new ModuleOrder { Name="ID降序",Order=1 },
                new ModuleOrder {Name="发布时间升序", Order=2 },
                new ModuleOrder { Name="发布时间降序",Order=3},
                new ModuleOrder { Name="点击升序",Order=4},
                new ModuleOrder { Name="点击降序",Order=5}
            }
                };
                modules.Add(module);
                moduleService.AddRange(modules.ToArray());
            }

           
        }

        public static void InitializeCategory(InterfaceCategoryService categoryService)
        {
            if (categoryService.FindList().Count() == 0)
            {
                var modules = new List<Category>();
                modules.Add(new Category
                {
                    Name = "公司简介",
                    View = "Index",
                    Type = CategoryType.General,
                    ParentId = 0,
                    ParentPath = "",
                    Order = 0,
                    Target = LinkTarget._self,
                    Description = "这是公司简介",
                    General = new CategoryGeneral { ContentView = "Index", ModuleId = 1, ContentOrder = 1 }
                });

                modules.Add(new Category
                {
                    Name = "单页栏目",
                    View = "Index",
                    Type = CategoryType.Page,
                    ParentId = 0,
                    ParentPath = "",
                    Order = 1,
                    Target = LinkTarget._self,
                    Description = "这是一个单页栏目",
                     Page = new CategoryPage  { Content="<p class=\"text-danger\">这是单页栏目内容</p>" }
                });

                modules.Add(new Category
                {
                    Name = "博客",
                    View = "Index",
                    Type = CategoryType.Link,
                    ParentId = 0,
                    ParentPath = "",
                    Order = 2,
                    Target = LinkTarget._self,
                    Description = "跳转到博客",
                     Link = new  CategoryLink {  Url="http://ninesky.cn" }
                });

                categoryService.AddRange(modules.ToArray());
            }
        }
    }
}
