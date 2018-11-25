using AutoMapper;
using Openbook.Bee.Core.AutoFac;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    /// <summary>
    /// 任务加工
    /// </summary>
    public class TaskProcess
    {

    }

    /// <summary>
    /// 创建T8任务基类
    /// </summary>
    public abstract class ACreateTask
    {
        protected T8ConfigItemEntity _T8ConfigItemEntity;
        protected T8ConfigEntity _T8ConfigEntity;

        //public ACreateTask(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity)
        //{
        //    this._T8ConfigEntity = t8ConfigEntity;
        //    this._T8ConfigItemEntity = t8ConfigItem;
        //}

        /// <summary>
        /// 设置初始化数据
        /// </summary>
        /// <param name="t8ConfigItem"></param>
        /// <param name="t8ConfigEntity"></param>
        public abstract void InitData(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity);      

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns></returns>
        public abstract T8TaskEntity CreateTask();

        protected virtual SqlQueryTimeStragety GetISqlQueryTimeInstance(DateType dateType)
        {
           
            ISqlQueryTime sqlQueryTime =null;
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

        protected virtual T8TaskEntity GetNewTask(TaskSourceType taskSourceType)
        {
            T8FileEntity t8FileEntity = Mapper.Map<T8FileEntity>(this._T8ConfigItemEntity);
            t8FileEntity.DbFileType = this._T8ConfigEntity.DbFileType;
            t8FileEntity.FtpInfo = this._T8ConfigEntity.FtpInfo;
            t8FileEntity.DataBaseInfo = this._T8ConfigEntity.DataBaseInfo;
            t8FileEntity.TaskSourceType = taskSourceType;

            var sqlQueryService = this.GetISqlQueryTimeInstance(this._T8ConfigItemEntity.DateType);
            t8FileEntity.SqlStartTime = sqlQueryService.GetStartTime(DateTime.Now);
            t8FileEntity.SqlEndTime = sqlQueryService.GetEndTime(DateTime.Now);

            T8TaskEntity t8TaskEntity = new T8TaskEntity();
            t8TaskEntity.TaskTitle = Common.GetT8TaskTitle(this._T8ConfigItemEntity.DateType, this._T8ConfigItemEntity.DataType);
            t8TaskEntity.GenerateTime = DateTime.Now;
            t8TaskEntity.T8TaskStatus = T8TaskStatus.Created;
            t8TaskEntity.TaskSourceType = taskSourceType;
            t8TaskEntity.T8FileEntity = t8FileEntity;
            return t8TaskEntity;
        }
    }

   /// <summary>
   /// 服务创建T8任务
   /// </summary>
    public class ServiceCreateTask : ACreateTask
    {
        //public ServiceCreateTask(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity) : base(t8ConfigItem, t8ConfigEntity)
        //{
        //}

        public override void InitData(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity)
        {
            base._T8ConfigEntity = t8ConfigEntity;
            base._T8ConfigItemEntity = t8ConfigItem;
        }

        public override T8TaskEntity CreateTask()
        {
            try
            {
                return base.GetNewTask(TaskSourceType.Service);
            }
            catch(Exception ex)
            {
                LogUtil.WriteLog($"ServiceCreateTask.T8TaskEntity()异常,({ex.Message})");
                return null;
            }           
        }
       
    }

    /// <summary>
    /// 用户手动操作创建T8任务
    /// </summary>
    public class UserCreateTask : ACreateTask
    {
        //public UserCreateTask(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity) : base(t8ConfigItem, t8ConfigEntity)
        //{
        //}

        public override void InitData(T8ConfigItemEntity t8ConfigItem, T8ConfigEntity t8ConfigEntity)
        {
            base._T8ConfigEntity = t8ConfigEntity;
            base._T8ConfigItemEntity = t8ConfigItem;
        }

        public override T8TaskEntity CreateTask()
        {
            try
            {
                return base.GetNewTask(TaskSourceType.User);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog($"ServiceCreateTask.T8TaskEntity()异常,({ex.Message})");
                return null;
            }
        }
    }

}
