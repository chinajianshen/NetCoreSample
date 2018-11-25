using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
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
        public static string GetT8TaskTitle(DateType dateType,DataType dataType)
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
    }
}
