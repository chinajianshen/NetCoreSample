using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client
{
    public partial class EncryptKeyConfigFrm : BaseFrm
    {
        private SystemConfigService service;
        private SystemConfigEntity systemConfigEntity;
        public EncryptKeyConfigFrm()
        {
            InitializeComponent();
            service = new SystemConfigService();
        }      

        private void EncryptKeyConfigFrm_Load(object sender, EventArgs e)
        {
            systemConfigEntity =  service.FindSystemConfig((int)SystemConfigs.EncryptKey);

            if (systemConfigEntity != null && !string.IsNullOrEmpty(systemConfigEntity.ExSetting01))
            {
                txtEncryptKey.Text = systemConfigEntity.ExSetting01;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEncryptKey.Text))
            {
                base.ShowMessage("客户密钥为必填项");
                return;
            }

            if (systemConfigEntity == null)
            {
                systemConfigEntity = new SystemConfigEntity();
                systemConfigEntity.SysConfigID = (int)SystemConfigs.EncryptKey;
                systemConfigEntity.SysConfigName = "客户密钥";
                systemConfigEntity.Status = 1;
            }

            systemConfigEntity.ExSetting01 = txtEncryptKey.Text.Trim();
            bool isSuccess = service.Update(systemConfigEntity);
            if (isSuccess)
            {
                base.ShowMessage("客户密钥配置成功");
                this.Close();
            }
            else
            {
                base.ShowErrorMessage("客户密钥配置失败");
            }

        }
    }
}
