using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Test
{
    public partial class JobFrm : Form
    {
        TaskService taskBll = new TaskService();
        FtpService ftpBll = new FtpService();
        private CancellationTokenSource cts = new CancellationTokenSource();

        public JobFrm()
        {
            InitializeComponent();
        }

        private void btnJobStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTaskID.Text))
            {
                MessageBox.Show("任务ID不能为空");
                return;
            }

            string taskID = txtTaskID.Text.Trim();

            TaskEntity taskEntity = taskBll.Find(taskID);
            if (taskEntity == null)
            {
                MessageBox.Show("任务ID系统中不存在");
                return;
            }

            FtpConfigEntity ftpConfigEntity = ftpBll.GetFirstFtpInfo();

            taskEntity.FtpConfig = ftpConfigEntity;

            //构造数据文件产品并执行
            DbFileProductDirector director = new DbFileProductDirector();
            ADbFileProductBuilder productBuilder = new DbFileProductBuilder();
            director.ConstructProduct(productBuilder);
            DbFileProduct product = productBuilder.GetDbFileProduct();
            product.Execute(taskEntity, cts.Token);
        }

        private void btnJobStop_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
