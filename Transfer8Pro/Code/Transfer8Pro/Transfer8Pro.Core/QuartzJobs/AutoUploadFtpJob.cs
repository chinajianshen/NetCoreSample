using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Utils;
using Transfer8Pro.Entity;
using Transfer8Pro.Core.Infrastructure;
using System.Threading;

namespace Transfer8Pro.Core.QuartzJobs
{
    [DisallowConcurrentExecution]
    public class AutoUploadFtpJob : IJob
    {
       private static SystemConfigService sysConfigService = null;
       private static FtpService ftpService = null;
        static AutoUploadFtpJob()
        {
            ftpService = new FtpService();
            sysConfigService = new SystemConfigService();
        }
        public void Execute(IJobExecutionContext context)
        {
            try
            {              
                CancellationToken ct = (CancellationToken)context.JobDetail.JobDataMap["CanellationTokenParam"];
                ct.ThrowIfCancellationRequested();

                SystemConfigEntity systemConfig = sysConfigService.FindSystemConfig((int)SystemConfigs.FtpUpoladService);
                if (systemConfig == null || systemConfig.Status == 0)
                {
                    return;
                }

              
                FtpConfigEntity ftpConfig = ftpService.GetFirstFtpInfo();
                if (ftpConfig == null)
                {
                    throw new Exception("传8未配置FTP信息");
                }

                if (!Directory.Exists(ftpConfig.ExportFileDirectory))
                {
                    throw new Exception($"传8配置的FTP目录[{ftpConfig.ExportFileDirectory}]不存在");
                }

                string fileBackupPath = Path.Combine(AppPath.DataFolder, ConfigHelper.GetConfig("UploadFileBackpath", "UploadFileBackup"), DateTime.Now.ToString("yyyyMM"));

                if (!Directory.Exists(fileBackupPath))
                {
                    Directory.CreateDirectory(fileBackupPath);
                }

                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(ftpConfig.ExportFileDirectory));
                ct.ThrowIfCancellationRequested();

                files.ForEach(sourceFile =>
                {
                    ct.ThrowIfCancellationRequested();
                    try
                    {
                        FtpHelper.UploadFile(ftpConfig, sourceFile);
                        string backupfileFullpath = Path.Combine(fileBackupPath, FileHelper.GetFileName(sourceFile));

                        //备份上传文件
                        FileHelper.CopyFile(sourceFile, backupfileFullpath);
                        Thread.Sleep(200);
                        File.Delete(sourceFile);

                        LogUtil.WriteLog($"数据文件[{sourceFile}]上传成功");
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog(ex.Message);
                    }                    
                });
            }
            catch (Exception ex)
            {
                //JobExecutionException jex = new JobExecutionException(ex);
                //jex.RefireImmediately = true;
                LogUtil.WriteLog($"FtpJob作业类异常，异常信息[{ex.Message}][{ex.StackTrace}]");
            }
        }
    }
}
