using EnterpriseDT.Net.Ftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core.Infrastructure
{
    public class FtpHelper
    {
        /// <summary>
        /// 连接FTP服务器
        /// </summary>
        /// <param name="ftpInfo"></param>
        /// <returns></returns>
        public static bool ConnectFtpServer(FtpConfigEntity ftpInfo)
        {
            try
            {
                using (FTPConnection ftpConn = new FTPConnection
                {
                    ServerAddress = ftpInfo.ServerAddress,
                    //ServerDirectory = ftpInfo.ServerDirectory,
                    UserName = ftpInfo.UserName,
                    Password = ftpInfo.UserPassword,
                    CommandEncoding = Encoding.GetEncoding("GBK")
                })
                {
                    ftpConn.Connect();
                    if (!ftpConn.DirectoryExists(ftpInfo.ServerDirectory))
                    {
                        throw new Exception($"FTP服务器连接成功，但FTP服务器不存在目录名[{ftpInfo.ServerDirectory}]");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string errMsg = $"FTP服务器连接失败，FTP信息[{JsonObj<FtpConfigEntity>.ToJson(ftpInfo)}],异常[{ ex.Message}]";
                LogUtil.WriteLog(errMsg);
                throw new Exception(errMsg);
            }
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftpInfo"></param>
        /// <param name="sourceFilepath"></param>
        public static void UploadFile(FtpConfigEntity ftpInfo,string sourceFilepath)
        {
            if (ftpInfo == null)
            {
                throw new Exception("FtpHelper.UploadFile()方法，参数ftpInfo为空");
            }

            if (string.IsNullOrEmpty(sourceFilepath))
            {
                throw new Exception("FtpHelper.UploadFile()方法，参数sourceFilepath为空");
            }

            if (!File.Exists(sourceFilepath))
            {
                throw new Exception($"FtpHelper.UploadFile()方法，参数sourceFilepath[{sourceFilepath}]指定文件路径不存在");
            }

            try
            {
                using (FTPConnection ftpConn = new FTPConnection
                {
                    ServerAddress = ftpInfo.ServerAddress,
                    ServerDirectory = ftpInfo.ServerDirectory,
                    UserName = ftpInfo.UserName,
                    Password = ftpInfo.UserPassword,
                    CommandEncoding = Encoding.GetEncoding("GB2312")
                })
                {
                    ftpConn.Connect();

                    string tempFtpFileName = FileHelper.GetFileName(sourceFilepath) + ".part";
                    string ftpFileName = FileHelper.GetFileName(sourceFilepath);

                    bool exist_file = ftpConn.Exists(tempFtpFileName);
                    if (exist_file)
                    {
                        LogUtil.WriteLog($"FTP服务器存在同名文件[{tempFtpFileName}],将ResumeNextTransfer");
                        ftpConn.ResumeNextTransfer();
                    }

                    ftpConn.UploadFile(sourceFilepath, tempFtpFileName, exist_file);
                    Thread.Sleep(200);
                    ftpConn.RenameFile(tempFtpFileName, ftpFileName);                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"数据文件[{sourceFilepath}]上传失败,异常信息[{ex.Message}][{ex.StackTrace}]");
            }
           
        }
    }
}
