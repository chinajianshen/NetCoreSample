using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Transfer8Pro.DAO;
using Transfer8Pro.DAO.DataHandlers;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.Infrastructure
{

    /// <summary>
    /// 构造产品指挥者
    /// </summary>
    public class DbFileProductDirector
    {
        private ADbFileProductBuilder _builder;
        /// <summary>
        /// 产品构造器
        /// </summary>
        /// <param name="builder"></param>
        public void ConstructProduct(ADbFileProductBuilder builder)
        {
            this._builder = builder;

            //1生成数据文件及数据
            _builder.BuildDbFile();

            //2压缩数据文件
            _builder.BuildCompressFile();

            //3压缩文件复制FTP指定文件目录
            _builder.CompressDbFileToFtpPath();            
        }
    }

    /// <summary>
    /// 数据库文件产品
    /// </summary>
    public class DbFileProduct
    {
        private List<Action<TaskEntity>> ProductParts { get; set; }

        public DbFileProduct()
        {
            ProductParts = new List<Action<TaskEntity>>();
        }

        /// <summary>
        /// 添加产品组件
        /// </summary>
        /// <param name="action"></param>
        public void AddPart(Action<TaskEntity> action)
        {
            ProductParts.Add(action);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="t8Task"></param>
        public void Execute(TaskEntity t8Task, CancellationToken ct)
        {
            foreach (Action<TaskEntity> item in ProductParts)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    item(t8Task);
                }
                catch
                {
                    break;
                }
            }
        }
    }


    /// <summary>
    /// 数据库文件产品抽象构造者类
    /// </summary>
    public abstract class ADbFileProductBuilder
    {
        /// <summary>
        /// 新建数据文件及数据
        /// </summary>       
        public abstract void BuildDbFile();

        /// <summary>
        /// 创建数据库压缩文件
        /// </summary>       
        /// <returns></returns>
        public abstract void BuildCompressFile();        

        /// <summary>
        /// 压缩文件复制到FTP指定目录
        /// </summary>      
        /// <returns></returns>
        public abstract void CompressDbFileToFtpPath();

        /// <summary>
        /// 生成数据文件产品
        /// </summary>
        /// <returns></returns>
        public abstract DbFileProduct GetDbFileProduct();

    }

    /// <summary>
    /// 数据库文件产品实现类
    /// </summary>
    public class DbFileProductBuilder : ADbFileProductBuilder
    {
        DbFileProduct product = new DbFileProduct();      

        public override void BuildDbFile()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    BuildInstanceObject buildInstanceService = new BuildInstanceObject();
                    //1得到数据库文件路径
                    AFileName fileNameStragety = buildInstanceService.GetGenerateFileNameStragety(1);
                    ISqlQueryTime SqlQueryTimeStragety = buildInstanceService.GetSqlQueryTimeStragety(t8Task.CycleType);

                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                    fileInfoEntity.NormalFileName = fileNameStragety.FileName(t8Task);
                    fileInfoEntity.NormalFilePath = fileNameStragety.FileFullName(t8Task);
                    fileInfoEntity.SqlStartTime = SqlQueryTimeStragety.StartTime(DateTime.Now);
                    fileInfoEntity.SqlEndTime = SqlQueryTimeStragety.EndTime(DateTime.Now);

                    t8Task.FileInfo = fileInfoEntity;

                    //2创建数据库文件并添加数据
                    IDataHandler handler = DataHandlerFactory.GetHandler(t8Task.DataHandler);  
                    handler.DBConnectStr = Common.DecryptData(t8Task.DBConnectString_Hashed);
                    string sql = t8Task.SQL;
                    bool isSuccess = handler.OutputData(sql, fileInfoEntity.SqlStartTime,fileInfoEntity.SqlEndTime, fileInfoEntity.NormalFilePath);
                    Thread.Sleep(300);
                    if (!isSuccess)
                    {
                        LogUtil.WriteLog($"从数据库向数据文件[{fileInfoEntity.NormalFilePath}]导入数据出现错误");
                        throw new Exception($"从数据库向数据文件[{fileInfoEntity.NormalFilePath}]导入数据出现错误");
                    }
                    
                    LogUtil.WriteLog($"数据文件[{fileInfoEntity.NormalFilePath}]创建并添加数据完成");
                }
                catch (Exception ex)
                {                   
                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }

            };
            product.AddPart(action);
        }

        public override void BuildCompressFile()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    if (!File.Exists(t8Task.FileInfo.NormalFilePath))
                    {
                        throw new Exception($"开始压缩操作，发现数据文件:[{t8Task.FileInfo.NormalFilePath}]不存在");
                    }
                    AFileName fileNameStragety = new BuildInstanceObject().GetGenerateFileNameStragety(2);
                    t8Task.FileInfo.CompressFileName = fileNameStragety.FileName(t8Task);
                    t8Task.FileInfo.CompressFilePath = fileNameStragety.FileFullName(t8Task);

                    //重命名一般文件
                    string newNormalFilePath = Path.Combine(FileHelper.GetDirectoryName(t8Task.FileInfo.NormalFilePath), FileHelper.GetFileNameWithoutExtension(t8Task.FileInfo.CompressFilePath));
                    if (!FileHelper.MoveFile(t8Task.FileInfo.NormalFilePath, newNormalFilePath))
                    {
                        throw new Exception($"开始压缩操作，重命名文件操作异常,源文件[{t8Task.FileInfo.NormalFilePath}],目标文件[{newNormalFilePath}]");
                    }

                    Thread.Sleep(300);
                    FileHelper.ZipFile(newNormalFilePath, t8Task.FileInfo.CompressFilePath, Common.GetEncryptKey());
                    Thread.Sleep(300);
                    try
                    {
                        //删除一般文件出错不影响整体，这里进行容错处理
                        File.Delete(newNormalFilePath);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"压缩数据操作完成，清理没有用处一般数据文件[{newNormalFilePath}]时出错,异常信息：[{ex.Message}]");
                    }                   

                    LogUtil.WriteLog($"压缩数据文件[{t8Task.FileInfo.CompressFilePath}]完成");
                }
                catch (Exception ex)
                {
                 
                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }
            };
            product.AddPart(action);
        }

        public override void CompressDbFileToFtpPath()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    if (t8Task.FtpConfig == null || string.IsNullOrEmpty(t8Task.FtpConfig.ExportFileDirectory))
                    {
                        LogUtil.WriteLog($"开始将数据压缩文件复制到FTP目录，发现客户未配置FTP导出文件夹信息");
                        return;
                    }

                    if (!Directory.Exists(t8Task.FtpConfig.ExportFileDirectory))
                    {
                        Directory.CreateDirectory(t8Task.FtpConfig.ExportFileDirectory);
                    }

                    if (!File.Exists(t8Task.FileInfo.CompressFilePath))
                    {
                        throw new Exception($"开始将数据压缩文件复制到FTP目录，发现数据压缩文件:[{t8Task.FileInfo.CompressFilePath}]不存在");
                    }

                    string ftpFilePath = Path.Combine(t8Task.FtpConfig.ExportFileDirectory, FileHelper.GetFileName(t8Task.FileInfo.CompressFilePath));

                    try
                    {
                        FileHelper.CopyFile(t8Task.FileInfo.CompressFilePath, ftpFilePath);
                        Thread.Sleep(300);                        
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"开始将数据压缩文件复制到FTP目录，复制过程中出现错误,源文件[{t8Task.FileInfo.CompressFilePath}],目的文件[{ftpFilePath}],异常信息[{ex.Message}][{ex.StackTrace}]");
                    }

                    try
                    {
                        File.Delete(t8Task.FileInfo.CompressFilePath);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"数据压缩文件[{t8Task.FileInfo.CompressFilePath}]复制到FTP目录已经完成，但删除该文件时出现异常(并不影响程序流程，所以按本任务已执行完毕)，异常信息[{ex.Message}][{ex.StackTrace}]");
                    }
                    LogUtil.WriteLog($"数据压缩文件复制到FTP目录[{ftpFilePath}]完成");
                }
                catch(Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }
            };

            product.AddPart(action);
        }

        public override DbFileProduct GetDbFileProduct()
        {
            return product;
        }

       
    }


    /// <summary>
    /// 传8数据库文件产品实现类
    /// </summary>
    public class T8DbFileProductBuilder : ADbFileProductBuilder
    {
        DbFileProduct product = new DbFileProduct();

        public override void BuildDbFile()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    BuildInstanceObject buildInstanceService = new BuildInstanceObject();
                    //1得到数据库文件路径
                    AFileName fileNameStragety = buildInstanceService.GetGenerateFileNameStragety(3);
                    ISqlQueryTime SqlQueryTimeStragety = buildInstanceService.GetSqlQueryTimeStragety(t8Task.CycleType);

                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                    fileInfoEntity.NormalFileName = fileNameStragety.FileName(t8Task);
                    fileInfoEntity.NormalFilePath = fileNameStragety.FileFullName(t8Task);
                    fileInfoEntity.SqlStartTime = SqlQueryTimeStragety.StartTime(DateTime.Now);
                    fileInfoEntity.SqlEndTime = SqlQueryTimeStragety.EndTime(DateTime.Now);

                    t8Task.FileInfo = fileInfoEntity;

                    //2创建数据库文件并添加数据
                    IDataHandler handler = DataHandlerFactory.GetHandler(t8Task.DataHandler);
                    handler.DBConnectStr = Common.DecryptData(t8Task.DBConnectString_Hashed);
                    string sql = t8Task.SQL;
                    bool isSuccess = handler.OutputDataToSQLite(sql, fileInfoEntity.SqlStartTime, fileInfoEntity.SqlEndTime, fileInfoEntity.NormalFilePath);
                    Thread.Sleep(300);
                    if (!isSuccess)
                    {
                        LogUtil.WriteLog($"从数据库向数据文件[{fileInfoEntity.NormalFilePath}]导入数据出现错误");
                        throw new Exception($"从数据库向数据文件[{fileInfoEntity.NormalFilePath}]导入数据出现错误");
                    }

                    LogUtil.WriteLog($"数据文件[{fileInfoEntity.NormalFilePath}]创建并添加数据完成");
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }

            };
            product.AddPart(action);
        }

        public override void BuildCompressFile()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    if (!File.Exists(t8Task.FileInfo.NormalFilePath))
                    {
                        throw new Exception($"开始压缩操作，发现数据文件:[{t8Task.FileInfo.NormalFilePath}]不存在");
                    }
                    AFileName fileNameStragety = new BuildInstanceObject().GetGenerateFileNameStragety(4);
                    t8Task.FileInfo.CompressFileName = fileNameStragety.FileName(t8Task);
                    t8Task.FileInfo.CompressFilePath = fileNameStragety.FileFullName(t8Task);

                    //重命名一般文件
                    string newNormalFilePath = Path.Combine(FileHelper.GetDirectoryName(t8Task.FileInfo.NormalFilePath), FileHelper.GetFileNameWithoutExtension(t8Task.FileInfo.CompressFilePath));
                    if (!FileHelper.MoveFile(t8Task.FileInfo.NormalFilePath, newNormalFilePath))
                    {
                        throw new Exception($"开始压缩操作，重命名文件操作异常,源文件[{t8Task.FileInfo.NormalFilePath}],目标文件[{newNormalFilePath}]");
                    }

                    Thread.Sleep(300);
                    FileHelper.ZipFile(newNormalFilePath, t8Task.FileInfo.CompressFilePath, Common.GetEncryptKey());
                    Thread.Sleep(300);
                    try
                    {
                        //删除一般文件出错不影响整体，这里进行容错处理
                        File.Delete(newNormalFilePath);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"压缩数据操作完成，清理没有用处一般数据文件[{newNormalFilePath}]时出错,异常信息：[{ex.Message}]");
                    }

                    LogUtil.WriteLog($"压缩数据文件[{t8Task.FileInfo.CompressFilePath}]完成");
                }
                catch (Exception ex)
                {

                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }
            };
            product.AddPart(action);
        }       

        public override void CompressDbFileToFtpPath()
        {
            Action<TaskEntity> action = t8Task =>
            {
                try
                {
                    if (t8Task.FtpConfig == null || string.IsNullOrEmpty(t8Task.FtpConfig.ExportFileDirectory))
                    {
                        LogUtil.WriteLog($"开始将数据压缩文件复制到FTP目录，发现客户未配置FTP导出文件夹信息");
                        return;
                    }

                    if (!Directory.Exists(t8Task.FtpConfig.ExportFileDirectory))
                    {
                        Directory.CreateDirectory(t8Task.FtpConfig.ExportFileDirectory);
                    }

                    if (!File.Exists(t8Task.FileInfo.CompressFilePath))
                    {
                        throw new Exception($"开始将数据压缩文件复制到FTP目录，发现数据压缩文件:[{t8Task.FileInfo.CompressFilePath}]不存在");
                    }

                    string ftpFilePath = Path.Combine(t8Task.FtpConfig.ExportFileDirectory, FileHelper.GetFileName(t8Task.FileInfo.CompressFilePath));

                    try
                    {
                        FileHelper.CopyFile(t8Task.FileInfo.CompressFilePath, ftpFilePath);
                        Thread.Sleep(300);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"开始将数据压缩文件复制到FTP目录，复制过程中出现错误,源文件[{t8Task.FileInfo.CompressFilePath}],目的文件[{ftpFilePath}],异常信息[{ex.Message}][{ex.StackTrace}]");
                    }

                    try
                    {
                        File.Delete(t8Task.FileInfo.CompressFilePath);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog($"数据压缩文件[{t8Task.FileInfo.CompressFilePath}]复制到FTP目录已经完成，但删除该文件时出现异常(并不影响程序流程，所以按本任务已执行完毕)，异常信息[{ex.Message}][{ex.StackTrace}]");
                    }

                    LogUtil.WriteLog($"数据压缩文件复制到FTP目录[{ftpFilePath}]完成");
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    throw new Exception(ex.Message);
                }
            };

            product.AddPart(action);
        }

        public override DbFileProduct GetDbFileProduct()
        {
            return product;
        }
    }
}
