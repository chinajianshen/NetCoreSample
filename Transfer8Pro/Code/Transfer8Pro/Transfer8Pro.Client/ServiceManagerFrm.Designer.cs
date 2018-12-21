namespace Transfer8Pro.Client
{
    partial class ServiceManagerFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDataStatus = new System.Windows.Forms.Label();
            this.lblFtpStatus = new System.Windows.Forms.Label();
            this.btnDataService = new System.Windows.Forms.Button();
            this.btnFtpService = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(32, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据导出服务：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(35, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "FTP上传服务：";
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.AutoSize = true;
            this.lblDataStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataStatus.ForeColor = System.Drawing.Color.Red;
            this.lblDataStatus.Location = new System.Drawing.Point(144, 29);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(52, 14);
            this.lblDataStatus.TabIndex = 2;
            this.lblDataStatus.Text = "未启动";
            // 
            // lblFtpStatus
            // 
            this.lblFtpStatus.AutoSize = true;
            this.lblFtpStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFtpStatus.ForeColor = System.Drawing.Color.Red;
            this.lblFtpStatus.Location = new System.Drawing.Point(144, 83);
            this.lblFtpStatus.Name = "lblFtpStatus";
            this.lblFtpStatus.Size = new System.Drawing.Size(52, 14);
            this.lblFtpStatus.TabIndex = 3;
            this.lblFtpStatus.Text = "未启动";
            // 
            // btnDataService
            // 
            this.btnDataService.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnDataService.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDataService.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDataService.Location = new System.Drawing.Point(229, 17);
            this.btnDataService.Name = "btnDataService";
            this.btnDataService.Size = new System.Drawing.Size(142, 40);
            this.btnDataService.TabIndex = 4;
            this.btnDataService.Tag = "0";
            this.btnDataService.Text = "启动";
            this.btnDataService.UseVisualStyleBackColor = false;
            this.btnDataService.Click += new System.EventHandler(this.btnDataService_Click);
            // 
            // btnFtpService
            // 
            this.btnFtpService.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnFtpService.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFtpService.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnFtpService.Location = new System.Drawing.Point(229, 71);
            this.btnFtpService.Name = "btnFtpService";
            this.btnFtpService.Size = new System.Drawing.Size(142, 40);
            this.btnFtpService.TabIndex = 5;
            this.btnFtpService.Tag = "0";
            this.btnFtpService.Text = "启动";
            this.btnFtpService.UseVisualStyleBackColor = false;
            this.btnFtpService.Click += new System.EventHandler(this.btnFtpService_Click);
            // 
            // ServiceManagerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 359);
            this.Controls.Add(this.btnFtpService);
            this.Controls.Add(this.btnDataService);
            this.Controls.Add(this.lblFtpStatus);
            this.Controls.Add(this.lblDataStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ServiceManagerFrm";
            this.Text = "服务管理";
            this.Load += new System.EventHandler(this.ServiceManagerFrm_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lblDataStatus, 0);
            this.Controls.SetChildIndex(this.lblFtpStatus, 0);
            this.Controls.SetChildIndex(this.btnDataService, 0);
            this.Controls.SetChildIndex(this.btnFtpService, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDataStatus;
        private System.Windows.Forms.Label lblFtpStatus;
        private System.Windows.Forms.Button btnDataService;
        private System.Windows.Forms.Button btnFtpService;
    }
}