using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.QuartzJobs
{
    /// <summary>
    /// 自动数据导出作业 
    /// </summary>
    [DisallowConcurrentExecution]  // DisallowConcurrentExecution属性标记任务不可并行，要是上一任务没运行完即使到了运行时间也不会运行
    public class AutoDataExportJob : IJob
    {
        private static TaskService taskService = null;
        private static FtpService ftpService = null;
        private static SystemConfigService sysConfigService = null;
        static AutoDataExportJob()
        {
            taskService = new TaskService();
            ftpService = new FtpService();
            sysConfigService = new SystemConfigService();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                CancellationToken ct = (CancellationToken)context.JobDetail.JobDataMap["CanellationTokenParam"];
                ct.ThrowIfCancellationRequested();

                //获取数据库所有有效任务，状态为（IsDelete=0（未删除））
                List<TaskEntity> taskList = taskService.GetAllTaskList();
                ct.ThrowIfCancellationRequested();

                //判断配置中是否开启服务
                SystemConfigEntity systemConfig = sysConfigService.FindSystemConfig((int)SystemConfigs.DataExportService);
                if (systemConfig == null || systemConfig.Status == 0)
                {
                    List<TaskEntity> currentTaskList = QuartzBase.GetCurrentTaskList();
                    if (QuartzBase.EnabledDataExportJob && currentTaskList.Count > 0)
                    {
                        foreach (TaskEntity item in currentTaskList)
                        {
                            try
                            {
                                if (item.Enabled == 1)
                                {
                                    QuartzBase.PauseJob(item.TaskID);
                                    taskService.UpdateTaskStatus(item.TaskID, TaskStatus.STOP);

                                    item.TaskStatus = TaskStatus.STOP;
                                    QuartzBase.UpdateTask(item);
                                }                               
                            }
                            catch (Exception ex)
                            {
                                LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                            }
                        }
                        QuartzBase.EnabledDataExportJob = false;
                    }                 
                    return;
                }
                else
                {
                    List<TaskEntity> currentTaskList = QuartzBase.GetCurrentTaskList();
                    if (!QuartzBase.EnabledDataExportJob && currentTaskList.Count>0)
                    {
                        foreach (TaskEntity item in currentTaskList)
                        {
                            try
                            {
                                if (item.Enabled == 1)
                                {
                                    QuartzBase.ResumeJob(item.TaskID);
                                    taskService.UpdateTaskStatus(item.TaskID, TaskStatus.RUN);

                                    item.TaskStatus = TaskStatus.RUN;
                                    QuartzBase.UpdateTask(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                            }
                        }
                        QuartzBase.EnabledDataExportJob = true;
                    }
                }
                
                ct.ThrowIfCancellationRequested();



                //添加新增任务
                List<TaskEntity> orginalTaskList = QuartzBase.GetCurrentTaskList().ToList();
                var addTaskList = (from task in taskList
                                  where  !(from orgtask in orginalTaskList select orgtask.TaskID).Contains(task.TaskID)
                                  select task).ToList();
                foreach (TaskEntity item in addTaskList)
                {
                    try
                    {
                        if (item.Enabled == 1)
                        {
                            item.TaskStatus = TaskStatus.RUN;
                            QuartzBase.ScheduleJob(item);                           
                            QuartzBase.AddTask(item);
                            taskService.UpdateTaskStatus(item.TaskID, TaskStatus.RUN);
                        }                       
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                    }
                }


                //任务状态变化处理（运行或暂停）
                orginalTaskList = QuartzBase.GetCurrentTaskList().ToList();
                var statusChangeTaskList = (from task in taskList
                                     from orgTask in orginalTaskList
                                     where task.TaskID == orgTask.TaskID && task.TaskUniqueCode == task.TaskUniqueCode && task.Enabled != orgTask.Enabled 
                                     select task).ToList();
                foreach (TaskEntity item in statusChangeTaskList)
                {
                    try
                    {
                        TaskEntity taskEntity = QuartzBase.GetCurrentTaskList().Find(task => task.TaskID == item.TaskID);                      
                        if (item.Enabled == 1)
                        {
                            if (taskEntity.TaskStatus == TaskStatus.STOP)
                            {
                                QuartzBase.ResumeJob(item.TaskID);
                                taskService.UpdateTaskStatus(item.TaskID, TaskStatus.RUN);                                

                                if (taskEntity != null)
                                {
                                    taskEntity.Enabled = item.Enabled;
                                    taskEntity.TaskStatus = TaskStatus.RUN;
                                    QuartzBase.UpdateTask(taskEntity);
                                }
                            }                         
                        }
                        else
                        {
                            if (taskEntity.TaskStatus == TaskStatus.RUN)
                            {
                                QuartzBase.PauseJob(item.TaskID);
                                taskService.UpdateTaskStatus(item.TaskID, TaskStatus.STOP);                             
                                if (taskEntity != null)
                                {
                                    taskEntity.Enabled = item.Enabled;
                                    taskEntity.TaskStatus = TaskStatus.STOP;
                                    QuartzBase.UpdateTask(taskEntity);
                                }
                            }                           
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                    }                   
                }

                ct.ThrowIfCancellationRequested();
                //任务内容变更处理
                orginalTaskList = QuartzBase.GetCurrentTaskList().ToList();
                var updateTaskList = (from task in taskList
                                      from orgTask in orginalTaskList
                                      where task.TaskID == orgTask.TaskID && task.Enabled == orgTask.Enabled && task.TaskUniqueCode != orgTask.TaskUniqueCode
                                      select new { NewTask = task,OriginTask = orgTask }).ToList();
                foreach (var item in updateTaskList)
                {
                    try
                    {
                        QuartzBase.ScheduleJob(item.NewTask, true);
                        taskService.UpdateTaskStatus(item.NewTask.TaskID, TaskStatus.RUN);

                        //修改原有任务信息     
                        item.NewTask.TaskStatus = TaskStatus.RUN;
                        QuartzBase.UpdateTask(item.NewTask);
                    }
                    catch(Exception ex)
                    {
                        LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                    }
                }

                ct.ThrowIfCancellationRequested();
                //已删除的任务
                orginalTaskList = QuartzBase.GetCurrentTaskList().ToList();
                List<TaskEntity> deletedTaskList = taskService.GetAllDeletedTaskList();
                var toDeleteTaskList = (from orgTask in orginalTaskList
                                        from task in deletedTaskList
                                       where task.TaskID == orgTask.TaskID
                                       select orgTask).ToList();
                foreach (TaskEntity item in toDeleteTaskList)
                {
                    try
                    {
                        QuartzBase.DeleteJob(item.TaskID);
                        taskService.UpdateTaskStatus(item.TaskID, TaskStatus.STOP);
                        TaskEntity taskEntity = QuartzBase.GetCurrentTaskList().Find(task => task.TaskID == item.TaskID);
                        if (taskEntity != null)
                        {                         
                            QuartzBase.DeleteTask(taskEntity.TaskID);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"{ex.Message}[{ex.StackTrace}]");
                    }

                }

                ct.ThrowIfCancellationRequested();               
            }
            catch (Exception ex)
            {             
                LogUtil.WriteLog($"自动处理作业AutoDealJob类出现异常，异常信息：[{ex.Message}][{ex.StackTrace}]");
            }
        }
    }

    public class TestJob1 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogUtil.WriteLog("TestJob1作业开始");

            
        }
    }
}
