using Autofac;
using OpenBook.Bee.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.AutoFac
{
    public class AutoFacConfiguration
    {
        public static void RegisterDependencies()
        {
            AutoFacContainer.Initialize(builder =>
            {
                RegisterClass(builder);
                RegisterAssbemly(builder);
            });
        }

        /// <summary>
        /// 注册具体依赖类
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterClass(ContainerBuilder builder)
        {
            //.InstancePerDependency()    //每次都创建一个对象。
            //.SingleInstance()   //每次都是同一个对象。
            //.InstancePerLifetimeScope()     //同一个生命周期生成的对象是同一个。
            // .InstancePerRequest()   //单个 Web/HTTP/API 请求的范围内的组件共享一个实例。仅可用于支持每个请求的依赖关系的整合（如MVC，Web API，Web Forms等）。

            // RegisterType方式 
            //builder.RegisterType<FirstModel>().As<IService>().InstancePerDependency();
            // Register方式
            //builder.Register(c => new SecondModel()).As<ISecondService>().InstancePerDependency();

            #region 测试数据
            //注册一个没有接口的类
            builder.RegisterType<ThirdModel>().AsSelf().InstancePerDependency();

            //注册一个接口多个实现 
            builder.RegisterType<FirstModel>().Named<IService>(typeof(FirstModel).Name);
            builder.RegisterType<FirstModel2>().Named<IService>(typeof(FirstModel2).Name);

            //注册构造函数有参数（没找到解决方法 ）
            //builder.RegisterType<AuotoModel>().As<IAutoService>().InstancePerDependency().PropertiesAutowired().InstancePerLifetimeScope();
            //builder.Register(c => new AuotoModel("123")).As<IAutoService>().InstancePerDependency().PropertiesAutowired();

            //抽象类及实现 需要手工指定
            builder.Register(c => new ATestModel()).As<ATest>().InstancePerDependency();

            //复杂对象注入  PropertiesAutowired 表示自动用已注册的填充属性
            builder.RegisterType<CInstence>().Named<IValidationRule>(typeof(CInstence).Name);
            builder.RegisterType<CInstence2>().Named<IValidationRule>(typeof(CInstence2).Name);
            builder.Register(c => new CClient()).As<CAbstract>().InstancePerDependency().PropertiesAutowired();
            #endregion

            #region SQL条件查询开始-结束时间 （多个实现）
            builder.RegisterType<MonthSqlQueryTime>().Named<ISqlQueryTime>(typeof(MonthSqlQueryTime).Name);
            builder.RegisterType<WeekSqlQueryTime>().Named<ISqlQueryTime>(typeof(WeekSqlQueryTime).Name);
            builder.RegisterType<DaySqlQueryTime>().Named<ISqlQueryTime>(typeof(DaySqlQueryTime).Name);
            #endregion

            #region 任务定时条件查询开始-结束时间（多个实现）
            builder.RegisterType<MonthTimingQueryTime>().Named<ITimingQueryTime>(typeof(MonthTimingQueryTime).Name);
            builder.RegisterType<WeekTimingQueryTime>().Named<ITimingQueryTime>(typeof(WeekTimingQueryTime).Name);
            builder.RegisterType<DayTimingQueryTime>().Named<ITimingQueryTime>(typeof(DayTimingQueryTime).Name);
            #endregion

            #region 生成文件名及其路径 （多个实现）
            builder.RegisterType<GeneralFileName>().Named<AFileName>(typeof(GeneralFileName).Name);
            builder.RegisterType<CompressFileName>().Named<AFileName>(typeof(CompressFileName).Name);
            builder.RegisterType<UploadBackFileName>().Named<AFileName>(typeof(UploadBackFileName).Name);
            #endregion

            #region 生成任务实体
            builder.RegisterType<ServiceCreateTask>().Named<ACreateTask>(typeof(ServiceCreateTask).Name);
            builder.RegisterType<UserCreateTask>().Named<ACreateTask>(typeof(UserCreateTask).Name);
            #endregion
        }

        /// <summary>
        /// 注册程序集
        /// 自动注入的问题
        /// 1一个接口多个实现（只保留最后一个实现）
        /// 2 抽象类不会自动注入 
        /// 此时就需要手工具体注入
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterAssbemly(ContainerBuilder builder)
        {
            // 自动注入的方式，不需要知道具体类的名称

            /* BuildManager.GetReferencedAssemblies()
             * 程序集的集合，包含 Web.config 文件的 assemblies 元素中指定的程序集、
             * 从 App_Code 目录中的自定义代码生成的程序集以及其他顶级文件夹中的程序集。
             */

            // 获取包含继承了IService接口类的程序集
            // var assemblies = 

            
            //AppDomain.CurrentDomain.GetAssemblies()

            string[] dlls = Directory.GetFiles(Directory.GetCurrentDirectory(), "OpenBook*.dll");
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var dll in dlls)
            {
                assemblies.Add(Assembly.LoadFile(dll));
            }

            builder.RegisterAssemblyTypes(assemblies.ToArray()).AsImplementedInterfaces();
        }
    }
}
