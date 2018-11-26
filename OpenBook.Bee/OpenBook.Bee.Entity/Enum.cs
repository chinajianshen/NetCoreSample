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
    public enum T8TaskStatus
    {
        Default =0,

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
        Default = 0,

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
        
        /// <summary>
        /// 备份上传文件
        /// </summary>
        BackupUploadFile = 8
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
        /// 天
        /// </summary>
        Day=4
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

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        SqlSever = 1,

        Oracle =2,

        MySql =4,
        
        Sybase =8,
     
        DB2 = 16,      
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 销售数据
        /// </summary>
        SaleData =1,

        /// <summary>
        /// 在架数据
        /// </summary>
        OnShelfData =2
    }

    /// <summary>
    /// 数据库文件类型
    /// </summary>
    public enum DbFileType
    {
        Access =1,
        SQLite =2
    }

    /// <summary>
    /// pos类型
    /// </summary>
    public enum PosType
    {
        自定义=0,
        金高=1
    }







    public enum LogTypes
    {
        /// <summary>
        /// 数据查询
        /// </summary>
        DataSearch = 1,
        /// <summary>
        /// 页面浏览
        /// </summary>
        PageBrowse = 2,
        /// <summary>
        /// 功能使用
        /// </summary>
        FunctionUsing = 3,
        /// <summary>
        /// 权限变更
        /// </summary>
        AuthorityChanged = 4,
        /// <summary>
        /// 错误日志
        /// </summary>
        SysError = 5,
        /// <summary>
        /// 系统日志
        /// </summary>
        SysLog = 6,
        /// <summary>
        /// 数据导出
        /// </summary>
        DataOutput = 7,
        /// <summary>
        /// Smart3.0日志
        /// </summary>
        PageLogs = 10,

    }

   
}
