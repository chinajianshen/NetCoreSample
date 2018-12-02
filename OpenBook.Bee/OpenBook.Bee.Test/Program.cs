using OpenBook.Bee.Test;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Entity;
using Openbook.Bee.Core.AutoFac;
using Openbook.Bee.Core.AutoMapper;
using System.Reflection;
using Common.Logging;
using OpenBook.Bee.Test.QuartzStudy;

namespace OpenBook.Bee.Tester
{
    class Program
    { 

        private static T8ConfigSetting t8ConfigSetting = new T8ConfigSetting();
        

        static Program()
        {
            AutoMapperConfiguration.Configure();
            AutoFacConfiguration.RegisterDependencies();
        }
        static void Main(string[] args)
        {
            //NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
            //log.Info("测试日志");
            //ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
            //logger.Info("测试日志");
            //logger.Error("错误日志");
            #region T8业务测试
            //生成配置文件
            //t8ConfigSetting.CreateConfig(true);          

            //获取配置文件
            //T8ConfigEntity t8Config =   T8ConfigHelper.T8Config;

            //T8ServiceTest t8test = new T8ServiceTest();
            //t8test.SqlQueryTimeStragety_Test();
            //t8test.TimingQueryTimeStragety_Test();
            //t8test.T8ConfigMapperT8FileEntity_Test();
            //t8test.CreateTaskEntity_Test();
            #endregion


            #region AutoMapper测试
            //MapperTest mapperTest = new MapperTest();
            //mapperTest.Test1();
            //mapperTest.Test2();
            //mapperTest.Test3();
            #endregion

            #region AutoFac测试
            //AutoFacTest autoFacTest = new AutoFacTest();
            //autoFacTest.Test1();
            #endregion

            #region Quartz测试
            //QuartzTest quartzTest = new QuartzTest();
            //quartzTest.Test1();

            //Chaper1 chaper1 = new Chaper1();
            //chaper1.test();


            Chaper2 chaper2 = new Chaper2();
            chaper2.Test();
            #endregion

            Console.Read();
        }
    }
}
