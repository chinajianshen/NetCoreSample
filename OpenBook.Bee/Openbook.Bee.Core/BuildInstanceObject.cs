using Openbook.Bee.Core.AutoFac;
using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    /// <summary>
    /// 构建实例化对象
    /// </summary>
    public class BuildInstanceObject
    {
        /// <summary>
        /// 获取定时循环开始和结束时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public virtual TimingQueryTimeStragety GetTimingQueryTimeStragety(DateType dateType)
        {
            ITimingQueryTime service = null;
            switch (dateType)
            {
                case DateType.Month:
                    service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(MonthTimingQueryTime).Name);
                    break;
                case DateType.Week:
                    service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(WeekTimingQueryTime).Name);
                    break;
                default:
                    service = AutoFacContainer.ResolveNamed<ITimingQueryTime>(typeof(DayTimingQueryTime).Name);
                    break;
            }

            return new TimingQueryTimeStragety(service);
        }

        /// <summary>
        /// 获取SQL查询条件开始和结束时间
        /// </summary>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public virtual SqlQueryTimeStragety GetSqlQueryTimeStragety(DateType dateType)
        {
            ISqlQueryTime sqlQueryTime = null;
            switch (dateType)
            {
                case DateType.Month:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(MonthSqlQueryTime).Name);
                    break;
                case DateType.Week:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(WeekSqlQueryTime).Name);
                    break;
                case DateType.Day:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(DaySqlQueryTime).Name);
                    break;
            }
            SqlQueryTimeStragety service = new SqlQueryTimeStragety(sqlQueryTime);
            return service;
        }

        /// <summary>
        /// 获取文件名信息 1一般文件 2压缩文件 3上传备份文件 
        /// </summary>
        /// <param name="type">1一般文件 2压缩文件 3上传备份文件</param>
        /// <returns></returns>
        public virtual GenerateFileNameStragety GetGenerateFileNameStragety(int type)
        {
            AFileName aFileName = null;
            switch (type)
            {
                case 1:
                    aFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(GeneralFileName).Name);
                    break;
                case 2:
                    aFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(CompressFileName).Name);
                    break;
                case 3:
                    aFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(UploadBackFileName).Name);
                    break;
            }
            GenerateFileNameStragety service = new GenerateFileNameStragety(aFileName);
            return service;
        }
    }
}
