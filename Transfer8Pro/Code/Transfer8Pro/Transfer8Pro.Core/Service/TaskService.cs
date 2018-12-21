using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer8Pro.DAO;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Core.Service
{
    public class TaskService
    {
        TaskDAO dao;
        public TaskService()
        {
            dao = new TaskDAO();
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
            return dao.GetTaskList(taskEntity, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>    
        /// <param name="pageIndex"></param>
        /// <param name="pageSizent"></param>
        /// <returns></returns>
        public ParamtersForDBPageEntity<TaskEntity> GetTaskList(int pageIndex, int pageSize)
        {
            return dao.GetTaskList(pageIndex, pageSize);
        }


        /// <summary>
        /// 读取数据库中全部的任务
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> GetAllTaskList()
        {
            return dao.GetAllTaskList();
        }
        /// <summary>
        /// 读取数据库已删除的全部任务
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> GetAllDeletedTaskList()
        {
            return dao.GetAllDeletedTaskList();
        }

        /// <summary>
        /// 新建 新建 1成功 0失败 2存在TaskName
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public int Insert(TaskEntity taskEntity)
        {
            return dao.Insert(taskEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public bool Update(TaskEntity taskEntity)
        {
            return dao.Update(taskEntity);
        }

        /// <summary>
        /// 删除 假删
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public bool Delete(string taskID)
        {
            return dao.Delete(taskID);
        }
        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public TaskEntity Find(string taskID)
        {
            return dao.Find(taskID);
        }


        /// <summary>
        /// 根据任务Id 修改 上次运行时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="recentRunTime"></param>
        /// <returns></returns>
        public bool UpdateRecentRunTime(string taskID, DateTime recentRunTime)
        {
            return dao.UpdateRecentRunTime(taskID, recentRunTime);
        }

        /// <summary>
        /// 修改任务的下次启动时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="nextFireTime"></param>
        /// <returns></returns>
        public bool UpdateNextFireTime(string taskID, DateTime nextFireTime)
        {
            return dao.UpdateNextFireTime(taskID, nextFireTime);
        }

        /// <summary>
        /// 更新任务状态  运行或停止
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public bool UpdateTaskStatus(string taskID, TaskStatus taskStatus)
        {
            return dao.UpdateTaskStatus(taskID, taskStatus);
        }

        /// <summary>
        /// 作业 停用/启用
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public bool UpdateTaskEnabledStatus(string taskID, int enabled)
        {
            return dao.UpdateTaskEnabledStatus(taskID, enabled);
        }
    }
}
