using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 任务类
    /// </summary>
    [Serializable]
    public class T8TaskEntity
    {
        public T8TaskEntity()
        {
            this.TaskGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 任务的ID，是一个随机生成的GUID
        /// </summary>
        public string TaskGuid { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskTitle { get; set; }        

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenerateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
         public T8TaskStatus T8TaskStatus { get; set; }        

        // 任务源类型
        /// </summary>
        public TaskSourceType TaskSourceType { get; set; }

        /// <summary>
        /// 执行失败的次数
        /// </summary>
        public int ExecFailureTime { get; set; }

        /// <summary>
        /// 记录附加信息 如上传成功备份过程中出现错误，并不影响功能可以忽略
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 传8数据文件类
        /// </summary>
        public T8FileEntity T8FileEntity { get; set; }
    }
}
