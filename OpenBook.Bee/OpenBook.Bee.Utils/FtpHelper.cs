﻿using EnterpriseDT.Net.Ftp;
using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    /// <summary>
    /// Ftp帮助类
    /// </summary>
    public class FtpHelper
    {
        /// <summary>
        /// 连接FTP服务器
        /// </summary>
        /// <param name="ftpInfo"></param>
        /// <returns></returns>
        public static bool ConnectFtpServer(FtpInfoEntity ftpInfo)
        {
            try
            {
                using (FTPConnection ftpConn = new FTPConnection {
                    ServerAddress = ftpInfo.ServerAddress,
                    ServerDirectory = ftpInfo.ServerDirectory,
                    UserName = ftpInfo.UserName,
                    Password = ftpInfo.UserPassword,
                    CommandEncoding = Encoding.GetEncoding("GBK")
                })
                {
                    ftpConn.Connect();                  
                }
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog($"FTP服务器连接失败，FTP信息[{JsonObj<FtpInfoEntity>.ToJson(ftpInfo)}],异常[{ ex.Message}]");
                return false;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftpInfo"></param>
        /// <param name="t8FileInfo"></param>
        public static void UploadFile(FtpInfoEntity ftpInfo,T8FileInfoEntity t8FileInfo)
        {
            using (FTPConnection ftpConn = new FTPConnection
            {
                ServerAddress = ftpInfo.ServerAddress,
                ServerDirectory = ftpInfo.ServerDirectory,
                UserName = ftpInfo.UserName,
                Password = ftpInfo.UserPassword,
                CommandEncoding = Encoding.GetEncoding("GBK")
            })
            {
                ftpConn.Connect();

                bool exist_file = ftpConn.Exists(t8FileInfo.FileName);
                if (exist_file)
                {
                    LogUtil.WriteLog($"FTP服务器存在同名文件[{t8FileInfo.FileName}],将ResumeNextTransfer");
                    ftpConn.ResumeNextTransfer();
                }

                ftpConn.UploadFile(t8FileInfo.FilePath, t8FileInfo.FileName,exist_file);
            }
        }
    }
}
