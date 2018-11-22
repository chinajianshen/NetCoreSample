using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        Created = 1,

        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Complete = 4,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 8,
    }

    /// <summary>
    /// 步骤状态
    /// </summary>
    public enum StepStatus
    {
        /// <summary>
        /// 生成文件
        /// </summary>
        GenerateFile = 1,

        /// <summary>
        /// 压缩文件
        /// </summary>
        CompressedFile = 2,

        /// <summary>
        /// 上传文件
        /// </summary>
        UploadFile =4,        
    }

    /// <summary>
    /// 日期类型
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// 月
        /// </summary>
        Month =1,

        /// <summary>
        /// 周
        /// </summary>
        Week=2,

        /// <summary>
        /// 年
        /// </summary>
        Year=4,

        /// <summary>
        /// 1-N
        /// </summary>
        CY=8,

        /// <summary>
        /// 天
        /// </summary>
        Day=16
    }

    /// <summary>
    /// 任务源类型
    /// </summary>
    public enum TaskSourceType
    {
        /// <summary>
        /// 服务
        /// </summary>
        Service =1,

        /// <summary>
        /// 用户手工生成
        /// </summary>
        User =2,
    }
}
