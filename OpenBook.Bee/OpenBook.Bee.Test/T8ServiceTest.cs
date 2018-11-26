using AutoMapper;
using Openbook.Bee.Core;
using Openbook.Bee.Core.AutoFac;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test
{
    /// <summary>
    /// 传8逻辑测试
    /// </summary>
   public class T8ServiceTest
    {
        /// <summary>
        /// 计算SQL语句开始和结束时间 
        /// </summary>
        public void SqlQueryTimeStragety_Test()
        {
            ISqlQueryTime service = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(MonthSqlQueryTime).Name);
            SqlQueryTimeStragety bll = new SqlQueryTimeStragety(service);
            Console.WriteLine($"ISqlQueryTime月开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");

             service = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(WeekSqlQueryTime).Name);
             bll = new SqlQueryTimeStragety(service);
            Console.WriteLine($"ISqlQueryTime周开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");

            service = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(DaySqlQueryTime).Name);
            bll = new SqlQueryTimeStragety(service);
            Console.WriteLine($"ISqlQueryTime日开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");

        }

        /// <summary>
        /// 计算定时任务开始和结束时间 
        /// </summary>
        public void TimingQueryTimeStragety_Test()
        {
            ITimingQueryTime service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(MonthTimingQueryTime).Name);
            TimingQueryTimeStragety bll = new TimingQueryTimeStragety(service);
            Console.WriteLine($"ITimingQueryTime月开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");

            service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(WeekTimingQueryTime).Name);
            bll = new TimingQueryTimeStragety(service);
            Console.WriteLine($"ITimingQueryTime周开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");

            service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(DayTimingQueryTime).Name);
            bll = new TimingQueryTimeStragety(service);
            Console.WriteLine($"ITimingQueryTime日开始时间：{bll.GetStartTime(DateTime.Now)}-结束时间：{bll.GetEndTime(DateTime.Now)}");
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        private void GenerateFileNameStragety_Test(T8FileEntity t8FileEntity)
        {
            AFileName service = AutoFacContainer.ResolveNamed<AFileName>(typeof(GeneralFileName).Name);
            GenerateFileNameStragety stragety = new GenerateFileNameStragety(service);            
            Console.WriteLine($"AFileName一般文件名{stragety.FileName(t8FileEntity)},全路径：{stragety.FileFullName(t8FileEntity)}");
        }

        /// <summary>
        /// t8配置类映射t8文件类
        /// </summary>
        public void T8ConfigMapperT8FileEntity_Test()
        {
            T8ConfigEntity t8Config = T8ConfigHelper.T8Config;
            T8FileEntity t8FileEntity = Mapper.Map<T8FileEntity>(t8Config);

            T8ConfigItemContainer container;           
            t8Config.T8ItemContainerDic.TryGetValue(DateType.Month,out container);
            T8FileEntity t8FileEntity2 = Mapper.Map<T8FileEntity>(container.T8ConfigItemSale);
            T8FileEntity t8FileEntity22 = Mapper.Map<T8FileEntity>(container.T8ConfigITemOnShelf);            
        }

        public void CreateTaskEntity_Test()
        {
            T8ConfigEntity t8Config = T8ConfigHelper.T8Config;
            T8FileEntity t8FileEntity = Mapper.Map<T8FileEntity>(t8Config);

            T8ConfigItemContainer container;
            t8Config.T8ItemContainerDic.TryGetValue(DateType.Month, out container);

            ACreateTask service = AutoFacContainer.ResolveNamed<ACreateTask>(typeof(ServiceCreateTask).Name);
            service.InitData(container.T8ConfigItemSale, t8Config);
            T8TaskEntity t8TaskEntity =  service.CreateTask();

            this.GenerateFileNameStragety_Test(t8TaskEntity.T8FileEntity);

            service.InitData(container.T8ConfigITemOnShelf, t8Config);
            t8TaskEntity = service.CreateTask();
            this.GenerateFileNameStragety_Test(t8TaskEntity.T8FileEntity);
        }
    }
}
