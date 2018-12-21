using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Core.Infrastructure
{
    public class BuildInstanceObject
    {
        /// <summary>
        /// 获取SQL查询条件开始和结束时间
        /// </summary>
        /// <param name="cycleType"></param>
        /// <returns></returns>
        public virtual ISqlQueryTime GetSqlQueryTimeStragety(CycleTypes cycleType)
        {
            ISqlQueryTime sqlQueryTime = null;
            switch (cycleType)
            {
                case  CycleTypes.M:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(MonthSqlQueryTime).Name);
                    break;
                case  CycleTypes.W:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(WeekSqlQueryTime).Name);
                    break;
                case  CycleTypes.D:
                    sqlQueryTime = AutoFacContainer.ResolveNamed<ISqlQueryTime>(typeof(DaySqlQueryTime).Name);
                    break;
                default:
                    throw new ArgumentNullException("GetSqlQueryTimeStragety()方法，参数cycleType为空");                    
            }
          
            return sqlQueryTime;
        }

        /// <summary>
        /// 获取文件名信息 1一般文件 2压缩文件  3 T8一般文件 4 T8压缩文件
        /// </summary>
        /// <param name="type">1一般文件 2压缩文件</param>
        /// <returns></returns>
        public virtual AFileName GetGenerateFileNameStragety(int type)
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
                    aFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(T8GeneralFileName).Name);
                    break;
                case 4:
                    aFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(T8CompressFileName).Name);
                    break;
                default:
                    throw new ArgumentNullException("GetSqlQueryTimeStragety()方法，参数type值错误");
            }  
            return aFileName;
        }
    }
}
