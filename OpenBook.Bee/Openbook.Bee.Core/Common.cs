using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    public class Common
    {
        /// <summary>
        /// 获取任务源目录
        /// </summary>
        /// <param name="taskSource"></param>
        /// <returns></returns>
        public static string GetDirName(TaskSourceType taskSource)
        {
            string dirName;
            switch (taskSource)
            {
                case TaskSourceType.Service:
                    dirName = "ServiceFileDir";
                    break;

                case TaskSourceType.User:
                    dirName = "UserFileDir";
                    break;
                default:
                    dirName = "ServiceFileDir";
                    break;
            }
            return dirName;
        }

        /// <summary>
        /// 获取子目录 
        /// </summary>
        /// <param name="type">1一般文件目录 2压缩文件目录 3上传备份文件目录</param>
        /// <returns></returns>
        public static string GetSubDirName(int type = 1)
        {
            string subDirName = "";
            if (type == 1)
            {
                //临时存放下一步成功时删除
                subDirName = "GeneralFileDir";
            }
            else if (type == 2)
            {
                //临时存放下一步成功时删除
                subDirName = "CompressFileDir";
            }
            else
            {
                subDirName = "UploadFileBackup";
            }
            return subDirName;
        }


        /// <summary>
        /// 获取日期类型后缀名称 
        /// 如 月 M
        /// </summary>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static string GetDateTypeName(DateType dateType)
        {
            string name = "";
            switch (dateType)
            {
                case DateType.Month:
                    name = "M";
                    break;
                case DateType.Week:
                    name = "W";
                    break;
                case DateType.Day:
                    name = "D";
                    break;
                default:
                    name = "M";
                    break;
            }
            return name;
        }

        /// <summary>
        /// 获取销售数据类型后缀名称
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string GetDataTypeName(DataType dataType)
        {
            string name = "";
            if (dataType == DataType.OnShelfData)
            {
                name = "S";
            }

            return name;
        }

        /// <summary>
        /// 得到数据文件扩展名
        /// </summary>
        /// <param name="dbFileType"></param>
        /// <returns></returns>
        public static string GetDBFileExtName(DbFileType dbFileType)
        {
            string dbExtName = "";
            switch (dbFileType)
            {
                case DbFileType.Access:
                    dbExtName = ".mdb";
                    break;
                case DbFileType.SQLite:
                    dbExtName = ".db";
                    break;
                default:
                    dbExtName = ".db";
                    break;
            }
            return dbExtName;
        }

        /// <summary>
        /// 生成T8任务标题
        /// </summary>
        /// <param name="dateType"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string GetT8TaskTitle(DateType dateType, DataType dataType)
        {
            string title = "";
            switch (dateType)
            {
                case DateType.Month:
                    title = "月";
                    break;
                case DateType.Week:
                    title = "周";
                    break;
                case DateType.Day:
                    title = "天";
                    break;
                default:
                    title = "月";
                    break;
            }

            if (dataType == DataType.SaleData)
            {
                title += "销售数据导出";
            }
            else
            {
                title += "在架数据导出";
            }
            return title;
        }


        /// <summary>
        /// 获取任务队列路径
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <returns></returns>
        public static string GetTaskQueueFileFullpath(TaskQueueType taskQueue)
        {
            string fullpath = "";
            switch (taskQueue)
            {
                case TaskQueueType.Completed:
                    fullpath = Path.Combine(AppPath.App_Root, "Completed_TaskQueue.dat");
                    break;
                case TaskQueueType.Processing:
                    fullpath = Path.Combine(AppPath.App_Root, "Processing_TaskQueue.dat");
                    break;
                case TaskQueueType.UserManual:
                    fullpath = Path.Combine(AppPath.App_Root, "UserManual_TaskQueue.dat");
                    break;
                case TaskQueueType.Error:
                    fullpath = Path.Combine(AppPath.App_Root, "Error_TaskQueue.dat");
                    break;
                default:
                    throw new ArgumentNullException($"获取任务队列路径失败,taskQueue参数为空");

            }
            return fullpath;
        }


        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="t8Task"></param>       
        /// <returns></returns>
        public static bool AddTaskToQueue(ConcurrentDictionary<string,T8TaskEntity> queue,T8TaskEntity t8Task, TaskQueueType taskQueue)
        {
            if (queue == null) return false;
            if (queue.TryAdd(t8Task.GenerateTaskQueueKey, t8Task))
            {
                string taskQueueFullpath = Common.GetTaskQueueFileFullpath(taskQueue);
                bool isAdd = SerializableHelper<List<T8TaskEntity>>.BinarySerializeFile(taskQueueFullpath, queue.Values.ToList());
                if (!isAdd)
                {
                    T8TaskEntity tempT8Task;
                    queue.TryRemove(t8Task.GenerateTaskQueueKey,out tempT8Task);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 队列中删除队列
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="t8Task"></param>
        /// <returns></returns>
        public static bool RemoveTaskFromQueue(ConcurrentDictionary<string,T8TaskEntity> queue, T8TaskEntity t8Task, TaskQueueType taskQueue)
        {
            if (queue == null) return false;

            T8TaskEntity tempT8Task;
            if (queue.TryRemove(t8Task.GenerateTaskQueueKey,out tempT8Task))
            {
                string taskQueueFullpath = Common.GetTaskQueueFileFullpath(taskQueue);
                bool isAdd = SerializableHelper<T8TaskEntity>.BinarySerializeFile(taskQueueFullpath, queue.Values.ToList());
                if (!isAdd)
                {                     
                    queue.TryRemove(t8Task.GenerateTaskQueueKey, out tempT8Task);
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 设置任务错误状态
        /// </summary>
        /// <param name="t8Task"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void SetTaskErrorStatus(T8TaskEntity t8Task, string msg)
        {
            t8Task.T8TaskStatus = T8TaskStatus.Error;
            t8Task.ExecFailureTime = t8Task.ExecFailureTime + 1;
            t8Task.Content = !string.IsNullOrEmpty(t8Task.Content) ? t8Task.Content + $"\r\n{msg}" : msg;
        }        
    }
}
