using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Core;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client
{
    public partial class FtpConfigFrm : BaseFrm
    {
        private FtpService ftpBll = null;

        public FtpConfigFrm()
        {
            InitializeComponent();
            ftpBll = new FtpService();
        }

        private void FtpConfigFrm_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnFtpConnCheck_Click(object sender, EventArgs e)
        {
            CheckFtpConnect();
        }

        private bool CheckFtpConnect()
        {
            if (Validate(true))
            {
                try
                {
                    FtpConfigEntity ftpConfig = BuildFtpConfigEntity();
                    FtpHelper.ConnectFtpServer(ftpConfig);
                    base.ShowMessage("FTP服务器连接成功");
                    return true;
                }
                catch (Exception ex)
                {
                    base.ShowErrorMessage(ex.Message);
                    return false;
                }
            }

            return false;
        }

        private FtpConfigEntity BuildFtpConfigEntity()
        {
            FtpConfigEntity ftpConfig = new FtpConfigEntity();
            ftpConfig.ServerAddress = txtServerAddress.Text.Trim();
            ftpConfig.ServerDirectory = txtServerDirectory.Text.Trim();
            ftpConfig.UserName = txtUserName.Text.Trim();
            ftpConfig.UserPassword = txtUserPassword.Text.Trim();
            ftpConfig.ExportFileDirectory = txtExportFileDirectory.Text.Trim();
            return ftpConfig;
        }

        private void btnScanDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            saveFileDialog.Description = "设置FTP导出文件夹";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtExportFileDirectory.Text = saveFileDialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {               
                FtpConfigEntity ftpConfig = BuildFtpConfigEntity();
                bool isSuccess = ftpBll.InsertOrUpdate(ftpConfig);
                if (isSuccess)
                {
                    base.ShowMessage("FTP配置成功");
                    this.Close();
                }
                else
                {
                    base.ShowErrorMessage("FTP配置失败");
                }
            }
        }

        private void BindData()
        {
            FtpConfigEntity ftpConfig = ftpBll.GetFirstFtpInfo();
            if (ftpConfig != null)
            {
                txtServerAddress.Text = ftpConfig.ServerAddress;
                txtServerDirectory.Text = ftpConfig.ServerDirectory;
                txtUserName.Text = ftpConfig.UserName;
                txtUserPassword.Text = ftpConfig.UserPassword;
                txtExportFileDirectory.Text = ftpConfig.ExportFileDirectory;
            }
        }

        private bool ValidateData(bool isFtpConnectCheck = false)
        {
            if (string.IsNullOrEmpty(txtServerAddress.Text))
            {
                base.ShowErrorMessage("FTP地址为必填项");
                return false;
            }

            if (string.IsNullOrEmpty(txtServerDirectory.Text))
            {
                base.ShowErrorMessage("FTP目录名为必填项");
                return false;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                base.ShowErrorMessage("账号为必填项");
                return false;
            }

            if (string.IsNullOrEmpty(txtUserPassword.Text))
            {
                base.ShowErrorMessage("密码为必填项");
                return false;
            }          

            if (!isFtpConnectCheck)
            {
                if (string.IsNullOrEmpty(txtExportFileDirectory.Text))
                {
                    base.ShowErrorMessage("导出文件夹配置为必选项");
                    return false;
                }

                if (!Directory.Exists(txtExportFileDirectory.Text))
                {
                    base.ShowErrorMessage("导出文件夹路径不存在，请检查文件路径地址");
                    return false;
                }
            }
           
            return true;
        }
    }
}
