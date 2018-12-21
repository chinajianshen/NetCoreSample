using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Transfer8Pro.Client
{
    public partial class TaskMainFrm : BaseFrm
    {
        public TaskMainFrm()
        {
            InitializeComponent();
            base.SetMdiParent(this);
        }

        private void TaskMainFrm_Load(object sender, EventArgs e)
        {
          
            //ServiceManagerFrm frm = new ServiceManagerFrm();
            //frm.MdiParent = this;
            //frm.Show();
        }      

        private void 设置服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            //base.ShowLoading();         
            ServiceManagerFrm frm = new ServiceManagerFrm();
            frm.MdiParent = this;
            frm.Show();
            //base.HideLoading();
        }

        private void 新建任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            AddTaskFrm frm = new AddTaskFrm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void 任务列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            TaskListFrm frm = new TaskListFrm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void fTP配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            FtpConfigFrm frm = new FtpConfigFrm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void 密钥配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            EncryptKeyConfigFrm frm = new EncryptKeyConfigFrm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void fTP上传历史列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            FtpUploadListFrm frm = new FtpUploadListFrm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void TaskMainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskManagerService.Stop();
            System.Environment.Exit(0);
        }

        private void TaskMainFrm_Shown(object sender, EventArgs e)
        {
            base.CheckSystemConfiguration();
            taskManagerService.Start();
        }
    }
}
