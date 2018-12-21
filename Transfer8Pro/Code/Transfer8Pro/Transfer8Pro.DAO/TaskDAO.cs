using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;
using Dapper;
using System.Text.RegularExpressions;
using System.Data;

namespace Transfer8Pro.DAO
{
    /*
         注意：
         1 Sqlite日期查询格式  datetime(createtime)<= datetime('2018-12-05 13:26:43')
         2 Sqlite日期保存或更新格式 taskEntity.CreateTime.ToString("s")
    */
    public class TaskDAO : DAOBase<SQLiteConnection>
    {

        public TaskDAO()
        {
            //string localDbConn = ConfigHelper.GetDBConnectStringConfig("LocalDBConnectStr");          
            base._default_connect_str = base.GetSqliteConnectString();
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSizent"></param>
        /// <returns></returns>
        public ParamtersForDBPageEntity<TaskEntity> GetTaskList(TaskEntity taskEntity, int pageIndex, int pageSize)
        {
            string sqlFormatter = "SELECT {2} FROM T8_Task WHERE IsDelete=0 {3} Order By CreateTime desc Limit {0} offset {0}*{1}";
            string sqlTotalFormatter = "SELECT COUNT(*) AS Cnt FROM T8_Task WHERE IsDelete=0 {0} Order By CreateTime desc";

            return base.ExecuteFor(conn =>
             {
                 List<TaskEntity> list = null;
                 int total = -1;
                 DynamicParameters prms = new DynamicParameters();
                 string sqlWhere = " AND 1=1 ";

                 if (taskEntity != null)
                 {

                     if (!string.IsNullOrEmpty(taskEntity.TaskID))
                     {
                         sqlWhere += " AND TaskID=@TaskID";
                         prms.Add("@TaskID", taskEntity.TaskID);
                     }

                     if (!string.IsNullOrEmpty(taskEntity.TaskName))
                     {
                         sqlWhere += string.Format(" AND TaskName LIKE '%{0}%'", taskEntity.TaskName);
                     }

                     if (taskEntity.DataType.ToString() != "0")
                     {
                         sqlWhere += " AND DataType=@DataType";
                         prms.Add("@DataType", taskEntity.DataType);
                     }

                     if (taskEntity.CycleType.ToString() != "0")
                     {
                         sqlWhere += " AND CycleType=@CycleType";
                         prms.Add("@CycleType", taskEntity.CycleType);
                     }

                     if (taskEntity.TaskStatus.ToString() != "0")
                     {
                         sqlWhere += " AND TaskStatus=@TaskStatus";
                         prms.Add("@TaskStatus", taskEntity.TaskStatus);
                     }

                     if (taskEntity.Enabled != 0)
                     {
                         sqlWhere += " AND Enabled=@Enabled";
                         prms.Add("@Enabled", taskEntity.Enabled);
                     }
                 }

                 string sql = string.Format(sqlFormatter, pageSize, pageIndex - 1, "*", sqlWhere);
                 string totalSql = string.Format(sqlTotalFormatter, sqlWhere);
                 list = conn.Query<TaskEntity>(sql, prms).ToList();

                 if (pageIndex == 1)
                 {
                     DataTable totalTable = conn.QueryDT(totalSql, prms);
                     total = totalTable.Rows[0]["Cnt"].ToString().ToInt();
                 }

                 return new ParamtersForDBPageEntity<TaskEntity>()
                 {
                     PageIndex = pageIndex,
                     PageSize = pageSize,
                     Total = total,
                     DataList = list
                 };
             });

        }


        /// <summary>
        /// 获取任务列表
        /// </summary>    
        /// <param name="pageIndex"></param>
        /// <param name="pageSizent"></param>
        /// <returns></returns>
        public ParamtersForDBPageEntity<TaskEntity> GetTaskList(int pageIndex, int pageSize)
        {
            return GetTaskList(null, pageIndex, pageSize);
        }

        /// <summary>
        /// 读取数据库中全部的任务
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> GetAllTaskList()
        {
            var sql = @"SELECT * FROM T8_Task WHERE IsDelete=0  Order By CreateTime desc;
                        SELECT * FROM T8_FtpConfig Limit 1;";

            return base.ExecuteFor((conn) =>
            {
                var multiResult = conn.QueryMultiple(sql);
                List<TaskEntity> list = multiResult.Read<TaskEntity>().ToList();
                FtpConfigEntity ftpConfigEntity = multiResult.Read<FtpConfigEntity>().FirstOrDefault();
                foreach (TaskEntity task in list)
                {
                    task.FtpConfig = ftpConfigEntity;
                }
                return list;
            });
        }

        /// <summary>
        /// 读取数据库已删除的全部任务
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> GetAllDeletedTaskList()
        {
            var sql = "SELECT TaskID FROM T8_Task WHERE IsDelete=1";
            return base.ExecuteFor(conn =>
            {
                return conn.Query<TaskEntity>(sql).ToList();
            });
        }

        /// <summary>
        /// 新建 1成功 0失败 2存在TaskName
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public int Insert(TaskEntity taskEntity)
        {
            var sql = @"INSERT INTO T8_Task (TaskID,TaskName,Cron,DataHandler,DBConnectString_Hashed,SQL,DataType,IsDelete,TaskStatus,CreateTime,ModifyTime,RecentRunTime,NextFireTime,Remark,Enabled,CycleType) 
                       VALUES(@TaskID,@TaskName,@Cron,@DataHandler,@DBConnectString_Hashed,@SQL,@DataType,@IsDelete,@TaskStatus,@CreateTime,@ModifyTime,@RecentRunTime,@NextFireTime,@Remark,@Enabled,@CycleType)";
            var existsSql = "SELECT COUNT(*) AS Cnt FROM T8_Task WHERE TaskName=@TaskName";

            return base.ExecuteFor((conn) =>
            {
                var prms = new
                {
                    TaskID = taskEntity.TaskID,
                    TaskName = taskEntity.TaskName,
                    Cron = taskEntity.Cron,
                    DataHandler = taskEntity.DataHandler,
                    DBConnectString_Hashed = taskEntity.DBConnectString_Hashed,
                    SQL = taskEntity.SQL,
                    DataType = taskEntity.DataType,
                    IsDelete = taskEntity.IsDelete,
                    TaskStatus = taskEntity.TaskStatus,
                    CreateTime = taskEntity.CreateTime != DateTime.MinValue ? taskEntity.CreateTime.ToString("s") : DateTime.Now.ToString("s"),
                    ModifyTime = taskEntity.ModifyTime != DateTime.MinValue ? taskEntity.ModifyTime.ToString("s") : null,
                    RecentRunTime = taskEntity.RecentRunTime != DateTime.MinValue ? taskEntity.RecentRunTime.ToString("s") : null,
                    NextFireTime = taskEntity.NextFireTime != DateTime.MinValue ? taskEntity.NextFireTime.ToString("s") : null,
                    Remark = taskEntity.Remark,
                    Enabled = taskEntity.Enabled != 0 ? taskEntity.Enabled : 1,
                    CycleType = taskEntity.CycleType
                };

                var cnt = conn.QueryDT(existsSql, prms).Rows[0]["Cnt"].ToString().ToInt();
                if (cnt > 0)
                {
                    return 2;
                }
                int result = conn.Execute(sql, prms);
                return result > 0 ? 1 : 0;
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public bool Update(TaskEntity taskEntity)
        {
            var sql = @"UPDATE T8_Task SET TaskName=@TaskName,Cron=@Cron,DataHandler=@DataHandler,DBConnectString_Hashed=@DBConnectString_Hashed,SQL=@SQL,DataType=@DataType,IsDelete=@IsDelete,TaskStatus=@TaskStatus,
                               CreateTime=@CreateTime,ModifyTime=@ModifyTime,RecentRunTime=@RecentRunTime,NextFireTime=@NextFireTime,Remark=@Remark,CycleType=@CycleType,Enabled=@Enabled WHERE TaskID=@TaskID";
            return base.ExecuteFor((conn) =>
            {
                var prms = new
                {
                    TaskID = taskEntity.TaskID,
                    TaskName = taskEntity.TaskName,
                    Cron = taskEntity.Cron,
                    DataHandler = taskEntity.DataHandler,
                    DBConnectString_Hashed = taskEntity.DBConnectString_Hashed,
                    SQL = taskEntity.SQL,
                    DataType = taskEntity.DataType,
                    IsDelete = taskEntity.IsDelete,
                    TaskStatus = taskEntity.TaskStatus,
                    CreateTime = taskEntity.CreateTime != DateTime.MinValue ? taskEntity.CreateTime.ToString("s") : null,
                    ModifyTime = taskEntity.ModifyTime != DateTime.MinValue ? taskEntity.ModifyTime.ToString("s") : null,
                    RecentRunTime = taskEntity.RecentRunTime != DateTime.MinValue ? taskEntity.RecentRunTime.ToString("s") : null,
                    NextFireTime = taskEntity.NextFireTime != DateTime.MinValue ? taskEntity.NextFireTime.ToString("s") : null,
                    Remark = taskEntity.Remark,
                    CycleType = taskEntity.CycleType,
                    Enabled = taskEntity.Enabled
                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 删除 假删
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public bool Delete(string taskID)
        {
            var sql = @"UPDATE T8_Task 
                        SET IsDelete=1
                        WHERE  TaskID=@TaskID";
            return base.ExecuteFor(conn =>
            {
                var prms = new
                {
                    TaskID = taskID
                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public TaskEntity Find(string taskID)
        {
            string sql = "SELECT * FROM T8_Task WHERE TaskID=@TaskID";
            return base.ExecuteFor(conn =>
            {
                var prms = new { TaskID = taskID };
                return conn.Query<TaskEntity>(sql, prms).FirstOrDefault();
            });
        }

        /// <summary>
        /// 根据任务Id 修改 上次运行时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="recentRunTime"></param>
        /// <returns></returns>
        public bool UpdateRecentRunTime(string taskID, DateTime recentRunTime)
        {
            var sql = @"UPDATE T8_Task 
                        SET RecentRunTime=@RecentRunTime
                        WHERE  TaskID=@TaskID";

            return base.ExecuteFor(conn =>
            {
                var prms = new
                {
                    RecentRunTime = recentRunTime.ToString("s"),
                    TaskID = taskID
                };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 修改任务的下次启动时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="nextFireTime"></param>
        /// <returns></returns>
        public bool UpdateNextFireTime(string taskID, DateTime nextFireTime)
        {
            var sql = @" UPDATE T8_Task
                           SET NextFireTime = @NextFireTime 
                               ,ModifyTime = @ModifyTime
                         WHERE TaskID=@TaskID";


            return base.ExecuteFor(conn =>
            {
                var prms = new { TaskID = taskID, ModifyTime = DateTime.Now.ToString("s"), NextFireTime = nextFireTime.ToString("s") };
                return conn.Execute(sql, prms) > 0;
            });
        }

        /// <summary>
        /// 更新任务状态  运行或停止
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public bool UpdateTaskStatus(string taskID, TaskStatus taskStatus)
        {
            var sql = @" UPDATE T8_Task
                         SET TaskStatus = @TaskStatus                              
                         WHERE TaskID=@TaskID";

            return base.ExecuteFor(conn =>
            {
                var prms = new { TaskID = taskID, TaskStatus = taskStatus };
                return conn.Execute(sql, prms) > 0;
            });
        }


        /// <summary>
        /// 作业 停用/启用
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public bool UpdateTaskEnabledStatus(string taskID, int enabled)
        {
            var sql = @" UPDATE T8_Task
                         SET Enabled = @Enabled                          
                         WHERE TaskID=@TaskID";
            return base.ExecuteFor(conn =>
            {
                var prms = new { TaskID = taskID, Enabled = enabled };
                return conn.Execute(sql, prms) > 0;
            });
        }
    }
}
