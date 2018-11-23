using OpenBook.Bee.Test;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Test.AutoMapperTest;
using OpenBook.Bee.Test.AutoFac;

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
            //生成配置文件
            //t8ConfigSetting.CreateConfig(true);          

            //获取配置文件
            //T8ConfigEntity t8Config =   T8ConfigHelper.T8Config;

            #region AutoMapper测试
            MapperTest mapperTest = new MapperTest();
            //mapperTest.Test1();
            //mapperTest.Test2();
            mapperTest.Test3();
            #endregion

            #region AutoFac测试
            AutoFacTest autoFacTest = new AutoFacTest();
            autoFacTest.Test1();
            #endregion

            Console.Read();
        }
    }
}
