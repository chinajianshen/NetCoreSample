using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Test
{
    public partial class GenerateFileNameFrm : Form
    {
        public GenerateFileNameFrm()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文本文件|*.jl";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("文件路径不能为空");
                return;
            }

            TaskEntity taskEntity = new TaskEntity();
            taskEntity.FileInfo = new FileInfoEntity();
            taskEntity.FileInfo.NormalFilePath = textBox1.Text;

            AFileName comperessFileName = AutoFacContainer.ResolveNamed<AFileName>(typeof(CompressFileName).Name);
            string fileNamePath = comperessFileName.FileFullName(taskEntity);
            textBox4.Text = fileNamePath;
            textBox2.Text = comperessFileName.FileName(taskEntity).Replace(".zip","");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("请先点击 生成文件名按钮");
                return;
            }
            if(!File.Exists(textBox4.Text))
            {
                MessageBox.Show("请先点击 生成压缩文件按钮");
                return;
            }

            try
            {
                FileHelper.UnZip(textBox4.Text, FileHelper.GetDirectoryName(textBox4.Text));
                MessageBox.Show("文件解压缩成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("请先点击 生成文件名按钮");
                    return;
                }
                    
                FileHelper.ZipFile(textBox1.Text, textBox4.Text);
                MessageBox.Show("压缩文件成功生成");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
