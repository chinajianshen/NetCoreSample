namespace Transfer8Pro.Test
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtTaskID = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.txtZipFilePath = new System.Windows.Forms.TextBox();
            this.txtUploadPath = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "加入一条任务数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(178, 52);
            this.button2.TabIndex = 1;
            this.button2.Text = "加密觖密";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(442, 30);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(178, 52);
            this.button3.TabIndex = 2;
            this.button3.Text = "更新任务数据";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtTaskID
            // 
            this.txtTaskID.Location = new System.Drawing.Point(626, 47);
            this.txtTaskID.Name = "txtTaskID";
            this.txtTaskID.Size = new System.Drawing.Size(199, 21);
            this.txtTaskID.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(25, 271);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(178, 52);
            this.button4.TabIndex = 4;
            this.button4.Text = "生成文件名";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(25, 432);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(178, 52);
            this.button5.TabIndex = 5;
            this.button5.Text = "执行作业";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(25, 104);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(178, 52);
            this.button6.TabIndex = 6;
            this.button6.Text = "加入一条FTP数据";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(25, 348);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(178, 52);
            this.button7.TabIndex = 7;
            this.button7.Tag = "0";
            this.button7.Text = "开始Quartz任务调度";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(442, 104);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(178, 52);
            this.button8.TabIndex = 8;
            this.button8.Text = "复制文件";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // txtZipFilePath
            // 
            this.txtZipFilePath.Location = new System.Drawing.Point(626, 121);
            this.txtZipFilePath.Name = "txtZipFilePath";
            this.txtZipFilePath.Size = new System.Drawing.Size(199, 21);
            this.txtZipFilePath.TabIndex = 9;
            // 
            // txtUploadPath
            // 
            this.txtUploadPath.Location = new System.Drawing.Point(626, 190);
            this.txtUploadPath.Name = "txtUploadPath";
            this.txtUploadPath.Size = new System.Drawing.Size(199, 21);
            this.txtUploadPath.TabIndex = 11;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(442, 173);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(178, 52);
            this.button9.TabIndex = 10;
            this.button9.Text = "上传文件";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(25, 499);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(178, 52);
            this.button10.TabIndex = 12;
            this.button10.Text = "Ftp上传测试";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 554);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.txtUploadPath);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.txtZipFilePath);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtTaskID);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "MainFrm";
            this.Text = "MainFrm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtTaskID;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox txtZipFilePath;
        private System.Windows.Forms.TextBox txtUploadPath;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}