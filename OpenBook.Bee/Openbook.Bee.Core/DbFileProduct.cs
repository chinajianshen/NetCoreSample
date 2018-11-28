using OpenBook.Bee.Database;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{

    /// <summary>
    /// 数据库文件产品
    /// </summary>
    public class DbFileProduct
    {
        private List<Action<T8TaskEntity>> ProductParts { get; set; }

        public DbFileProduct()
        {
            ProductParts = new List<Action<T8TaskEntity>>();
        }

        /// <summary>
        /// 添加产品组件
        /// </summary>
        /// <param name="action"></param>
        public void AddPart(Action<T8TaskEntity> action)
        {
            ProductParts.Add(action);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="t8Task"></param>
        public void Execute(T8TaskEntity t8Task, CancellationToken ct)
        {
            t8Task.T8TaskStatus = T8TaskStatus.Executing;
            foreach (Action<T8TaskEntity> item in ProductParts)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    item(t8Task);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog($"数据产品构造过程中出现异常：{ex.Message}");
                    LogUtil.WriteLog(ex);
                    break;
                }
            }
        }
    }

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

            //1生成数据库文件及数据
            _builder.BuildDbFile();

            //2压缩数据库文件
            _builder.BuildCompressFile();

            //3上传数据库压缩文件
            _builder.BuildUploadFile();

            //4备份上传成功文件
            _builder.BackupDbFile();
        }
    }

    /// <summary>
    /// 数据库文件产品抽象构造者类
    /// </summary>
    public abstract class ADbFileProductBuilder
    {
        /// <summary>
        /// 新建数据库文件及数据
        /// </summary>       
        public abstract void BuildDbFile();

        /// <summary>
        /// 创建数据库压缩文件
        /// </summary>       
        /// <returns></returns>
        public abstract void BuildCompressFile();

        /// <summary>
        /// 上传文件
        /// </summary>      
        /// <returns></returns>
        public abstract void BuildUploadFile();

        /// <summary>
        /// 备份数据库文件
        /// </summary>      
        /// <returns></returns>
        public abstract void BackupDbFile();

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
            Action<T8TaskEntity> action = t8Task =>
            {
                try
                {
                    t8Task.T8TaskStatus = T8TaskStatus.Executing;

                    //1得到数据库文件路径
                    GenerateFileNameStragety fileStragety = new BuildInstanceObject().GetGenerateFileNameStragety(1);
                    T8FileInfoEntity fileInfoEntity = new T8FileInfoEntity();
                    fileInfoEntity.FileName = fileStragety.FileName(t8Task.T8FileEntity);
                    fileInfoEntity.FilePath = fileStragety.FileName(t8Task.T8FileEntity);
                    fileInfoEntity.FileGenerateTime = DateTime.Now;
                    t8Task.T8FileEntity.GeneralFileInfo = fileInfoEntity;                  


                    //2创建数据库文件并添加数据
                    IDatabase databaseService = DatabaseFactory.CreateDatabase(t8Task.T8FileEntity.DataBaseInfo);
                    databaseService.ExecuteDataToDBFile(t8Task.T8FileEntity.DbFileType,fileInfoEntity.FilePath, t8Task.T8FileEntity.SqlString, t8Task.T8FileEntity.SqlStartTime, t8Task.T8FileEntity.SqlEndTime);
                                      

                    t8Task.T8FileEntity.StepStatus = StepStatus.GenerateFile;
                    LogUtil.WriteLog($"数据库文件[{fileInfoEntity.FilePath}]创建并添加数据完成");
                }
                catch (Exception ex)
                {                    
                    SetTaskErrorStatus(t8Task, $"BuildDbFile()[{ex.Message}]");
                    LogUtil.WriteLog(ex);
                }       

            };
            product.AddPart(action);                 
        }

        public override void BuildCompressFile()
        {
            Action<T8TaskEntity> action = t8Task =>
            {
                try
                {
                    if (!File.Exists(t8Task.T8FileEntity.GeneralFileInfo.FilePath))
                    {
                        throw new Exception($"数据库文件:{t8Task.T8FileEntity.GeneralFileInfo.FilePath}不存在,出现严重错误");
                    }

                    GenerateFileNameStragety fileStragety = new BuildInstanceObject().GetGenerateFileNameStragety(2);
                    T8FileInfoEntity fileInfoEntity = new T8FileInfoEntity();
                    fileInfoEntity.FileGenerateTime = DateTime.Now;
                    fileInfoEntity.FileName = fileStragety.FileName(t8Task.T8FileEntity);
                    fileInfoEntity.FilePath = fileStragety.FileFullName(t8Task.T8FileEntity);

                    FileHelper.ZipFile(t8Task.T8FileEntity.GeneralFileInfo.FilePath, fileInfoEntity.FilePath);

                    t8Task.T8FileEntity.StepStatus = StepStatus.CompressedFile;

                    LogUtil.WriteLog($"压缩数据库文件[{fileInfoEntity.FilePath}]完成");
                }
                catch (Exception ex)
                {
                    SetTaskErrorStatus(t8Task, $"BuildCompressFile()[{ex.Message}]");
                    LogUtil.WriteLog(ex);
                }
            };
            product.AddPart(action);
        }

        public override void BuildUploadFile()
        {
            Action<T8TaskEntity> action = t8Task =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    SetTaskErrorStatus(t8Task, $"BuildUploadFile()[{ex.Message}]");
                    LogUtil.WriteLog(ex);
                }
            };
            product.AddPart(action);
        }

        public override void BackupDbFile()
        {
            //备份文件异常不影响程序功能，所以出现异常也会默认任务成功，只是在任务中做个备注
            Action<T8TaskEntity> action = t8Task =>
            {
                try
                {
                    if (!File.Exists(t8Task.T8FileEntity.CompressFileInfo.FilePath))
                    {
                        throw new FileNotFoundException($"上传文件:{t8Task.T8FileEntity.CompressFileInfo.FilePath}不存在");
                    }


                    GenerateFileNameStragety fileStragety = new BuildInstanceObject().GetGenerateFileNameStragety(3);
                    T8FileInfoEntity fileInfoEntity = new T8FileInfoEntity();
                    fileInfoEntity.FileGenerateTime = DateTime.Now;
                    fileInfoEntity.FileName = fileStragety.FileName(t8Task.T8FileEntity);
                    fileInfoEntity.FilePath = fileStragety.FileFullName(t8Task.T8FileEntity);
                    t8Task.T8FileEntity.UploadBackFileInfo = fileInfoEntity;

                    bool isMoveSucess = FileHelper.MoveFile(t8Task.T8FileEntity.CompressFileInfo.FilePath, fileInfoEntity.FilePath);
                    if (!isMoveSucess)
                    {
                        throw new Exception($"压缩文件{t8Task.T8FileEntity.CompressFileInfo.FilePath}备份失败");
                    }

                    t8Task.T8FileEntity.StepStatus = StepStatus.BackupUploadFile;
                    t8Task.CompleteTime = DateTime.Now;
                    t8Task.T8TaskStatus = T8TaskStatus.Complete;
                }
                catch (Exception ex)
                {
                    t8Task.CompleteTime = DateTime.Now;
                    t8Task.T8TaskStatus = T8TaskStatus.Complete;
                    t8Task.Content = ex.Message;
                    LogUtil.WriteLog(ex.Message);
                }
            };
            product.AddPart(action);
        }
       

        public override DbFileProduct GetDbFileProduct()
        {
            return product;
        }

        private void SetTaskErrorStatus(T8TaskEntity t8Task,string msg)
        {
            t8Task.T8TaskStatus = T8TaskStatus.Error;
            t8Task.ExecFailureTime = t8Task.ExecFailureTime + 1;
            t8Task.Content = !string.IsNullOrEmpty(t8Task.Content) ? $"\r\n{msg}" : msg;
        }
    }
}
