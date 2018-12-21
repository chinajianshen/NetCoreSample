using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Client
{
    public partial class ServiceManagerFrm : BaseFrm
    {
        private SystemConfigService systemConfigBll = null;
        private FtpService ftpBll = null;
       
        public ServiceManagerFrm()
        {
            InitializeComponent();
            systemConfigBll = new SystemConfigService();
            ftpBll = new FtpService();               
        }

        private void ServiceManagerFrm_Load(object sender, EventArgs e)
        {        
            base.UIAction(() => {             
                BindData();               
            });        
        }

        private void btnDataService_Click(object sender, EventArgs e)
        {
            int status = ((Button)sender).Tag.ToString().ToInt() == 0 ? 1:0;
            string name = status == 1 ? "启动" : "停止";
            if (systemConfigBll.UpdateConfigStatus((int)SystemConfigs.DataExportService, status))
            {
                setServiceStatus(SystemConfigs.DataExportService, status);
            }
            else
            {
                base.ShowErrorMessage($"数据导出服务{name}失败");
                return;
            }
        }

        private void btnFtpService_Click(object sender, EventArgs e)
        {
            int status = ((Button)sender).Tag.ToString().ToInt() == 0 ? 1 : 0;
            string name = status == 1 ? "启动" : "停止";
            if (systemConfigBll.UpdateConfigStatus((int)SystemConfigs.FtpUpoladService, status))
            {
                setServiceStatus(SystemConfigs.FtpUpoladService, status);
            }
            else
            {
                base.ShowErrorMessage($"FTP上传服务{name}失败");
                return;
            }
        }

        private void BindData()
        {
            try
            {
                List<SystemConfigEntity> systemConfigList = systemConfigBll.GetSystemConfigList();
                if (systemConfigList.Count == 0)
                {
                    base.ShowErrorMessage("从系统数据库中未读取到配置数据");
                    return;
                }

                foreach (SystemConfigEntity item in systemConfigList)
                {
                    switch (item.SysConfigID)
                    {
                        case (int)SystemConfigs.DataExportService:
                            setServiceStatus(SystemConfigs.DataExportService, item.Status);
                            break;
                        case (int)SystemConfigs.FtpUpoladService:
                            if (item.Status == 1 && !checkFtpUseable())
                            {
                                return;             
                            }
                            setServiceStatus(SystemConfigs.FtpUpoladService, item.Status);
                            break;
                        case (int)SystemConfigs.HeartbeatService:
                            break;
                        case (int)SystemConfigs.ConfigSynStatus:
                            break;
                        case (int)SystemConfigs.SystemVersion:
                            break;                        
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"数据加载异常[{ex.Message}][{ex.StackTrace}]");
            }
        }

        private bool checkFtpUseable()
        {          
            FtpConfigEntity ftpConfig = ftpBll.GetFirstFtpInfo();
            if (ftpConfig == null)
            {
                base.ShowErrorMessage("您未配置FTP信息，请点击[FTP配置]菜单进行配置");
                return false; 
            }

            if (!FtpHelper.ConnectFtpServer(ftpConfig))
            {
                base.ShowErrorMessage("FTP配置信息错误，未能连接上FTP服务器");
                return false;
            }

            return true;
        }


        private void setServiceStatus(SystemConfigs systemConfig,int status)
        {
            switch (systemConfig)
            {
                case SystemConfigs.DataExportService:
                    if (status == 1)
                    {
                        lblDataStatus.Text = "运行中";
                        lblDataStatus.ForeColor = Color.MediumBlue;
                        btnDataService.Text = "停止";
                        btnDataService.Tag = 1;
                    }
                    else
                    {
                        lblDataStatus.Text = "未启动";
                        lblDataStatus.ForeColor = Color.Red;
                        btnDataService.Text = "启动";
                        btnDataService.Tag = 0;
                    }
                    break;
                case SystemConfigs.FtpUpoladService:
                    if (status == 1)
                    {
                        lblFtpStatus.Text = "运行中";
                        lblFtpStatus.ForeColor = Color.MediumBlue;
                        btnFtpService.Text = "停止";
                        btnFtpService.Tag = 1;
                    }
                    else
                    {
                        lblFtpStatus.Text = "未启动";
                        lblFtpStatus.ForeColor = Color.Red;
                        btnFtpService.Text = "启动";
                        btnFtpService.Tag = 0;
                    }
                    break;
                case SystemConfigs.HeartbeatService:
                    if (status == 1)
                    {

                    }
                    else
                    {

                    }
                    break;
            }
        }
       
    }
}
