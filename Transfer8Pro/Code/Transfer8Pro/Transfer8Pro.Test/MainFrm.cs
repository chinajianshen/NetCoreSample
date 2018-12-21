using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transfer8Pro.Core;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.QuartzJobs;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Test
{
    public partial class MainFrm : Form
    {
        TaskManagerService service = new TaskManagerService();
        public MainFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Core.Service.TaskService taskBll = new Core.Service.TaskService();

            //新加
            TaskEntity taskEntity = InsertTask();

            int result = taskBll.Insert(taskEntity);
            if (result == 1)
            {
                MessageBox.Show(string.Format("添加数据成功任务ID[{0}],任务名[{1}]",taskEntity.TaskID,taskEntity.TaskName));
            }
            else if (result == 2)
            {
                MessageBox.Show("系统中存在相同的任务名称");
            }
            else
            {
                MessageBox.Show("任务添加失败");
            }
           
        }

        private TaskEntity InsertTask()
        {
             TaskEntity taskEntity = new TaskEntity();
            taskEntity.TaskID = Guid.NewGuid().ToString("N");
            taskEntity.DataType = DataTypes.Sale;
            taskEntity.Cron = "0 3 * * * ? *";
            taskEntity.DataHandler = "Transfer8Pro.DAO.DataHandlers.SqlServer_DataHandler";
            string connStr = @"server=192.168.0.14;database=Smart_NewBookDB;uid=sa;pwd=sa.;min pool size=10;max pool size=300;Connection Timeout=10;";
            string encryptKey = Common.GetEncryptKey();
            taskEntity.DBConnectString_Hashed = RijndaelCrypt.Encrypt(connStr, encryptKey);
            taskEntity.SQL = "SELECT * FROM dbo.T8_BookInfo WHERE SalesDateTime>=@StartTime AND SalesDateTime<=@EndTime";
            taskEntity.TaskName = "天销售数据" + DateTime.Now.ToLongTimeString();
            //taskEntity.Enabled = true;
            taskEntity.IsDelete = false;
            taskEntity.TaskStatus = TaskStatus.RUN;
            taskEntity.CreateTime = DateTime.Now;
            return taskEntity;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EncryptFrm frm = new EncryptFrm();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string taskID = txtTaskID.Text.Trim();
            if (string.IsNullOrEmpty(taskID))
            {
                MessageBox.Show("请在右边输入TaskID");
                return;
            }

            Core.Service.TaskService taskBll = new Core.Service.TaskService();

            //查找一条数据
            //TaskEntity taskEntity = taskBll.Find(taskID);
            //if (taskEntity == null)
            //{
            //    MessageBox.Show(string.Format("TaskID[{0}]数据库不存在",taskID));
            //    return;
            //}            


            //更新           
            //taskEntity.ModifyTime = DateTime.Now;
            //taskEntity.Remark = DateTime.Now.ToString();
            //bool isSuccess2 = taskBll.Update(taskEntity);
            //if (isSuccess2)
            //{

            //}

            //获取任务列表
            //TaskEntity taskEntity2 = new TaskEntity();
            ParamtersForDBPageEntity<TaskEntity> pageTask = taskBll.GetTaskList(null, 1, 2);
            ParamtersForDBPageEntity<TaskEntity> pageTask2 = taskBll.GetTaskList(null, 2, 2);
            //List<TaskEntity> taskList = taskBll.GetAllTaskList();

            //更新上次运行时间
            //bool isResult1 = taskBll.UpdateRecentRunTime(taskID, DateTime.Parse("2018-12-04 02:25:25"));

            //更新下次运行时间
            //bool isResult3 = taskBll.UpdateNextFireTime(taskID, DateTime.Now.AddSeconds(1));

            //更新任务状态
            //bool isResult2 = taskBll.UpdateTaskStatus(taskID, TaskStatus.STOP);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GenerateFileNameFrm frm = new GenerateFileNameFrm();
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            JobFrm frm = new JobFrm();
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Core.Service.FtpService bll = new Core.Service.FtpService();
            FtpConfigEntity ftpConfig = new FtpConfigEntity();

            MessageBox.Show("先设置FTP文件夹目录");
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            saveFileDialog.Description = "设置FTP文件夹目录";
           if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
               
                ftpConfig.ServerAddress = "ftp://openbook.cn";
                ftpConfig.UserName = "t8jyqssd";
                ftpConfig.UserPassword = Common.EncryptData("t8jyqssd");
                ftpConfig.ExportFileDirectory = saveFileDialog.SelectedPath;
            }

            bool state = bll.InsertOrUpdate(ftpConfig);
            if (state)
            {
                MessageBox.Show("添加成功");
;            }
            else 
            {
                MessageBox.Show("添加失败");
            }          
        }

        private void button7_Click(object sender, EventArgs e)
        {            
            Button btn = (Button)sender;
            int status = btn.Tag.ToString().ToInt();
            if (status == 0)
            {              
                btn.Tag = 1;
                btn.Text = "结束Quartz任务调度";
                service.Start();
            }
            else
            {
                service.Stop();
                btn.Text = "开始Quartz任务调度";
                btn.Tag = 0;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtZipFilePath.Text))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtZipFilePath.Text = openFileDialog.FileName;
                }
            }

            string destPath = Path.Combine(AppPath.DataFolder, "TestZipFile");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            string destFile = Path.Combine(destPath, FileHelper.GetFileName(txtZipFilePath.Text));

            FileHelper.CopyFile(txtZipFilePath.Text, destFile);
            //FileHelper.CopyFilePlus(txtZipFilePath.Text, destFile);


            //加解密
            //destFile = destFile + ".zip";
            //FileHelper.ZipFile(txtZipFilePath.Text, destFile,Common.GetEncryptKey());
            //解密
            //FileHelper.UnZip(destFile, destPath, Common.GetEncryptKey());

            MessageBox.Show("文件复制成功");

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUploadPath.Text))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtUploadPath.Text = openFileDialog.FileName;
                }
            }
            FtpConfigEntity ftpConfig = new FtpService().GetFirstFtpInfo();

            try
            {
                FtpHelper.UploadFile(ftpConfig, txtUploadPath.Text);
                MessageBox.Show("上传成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传失败");
            }           
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
