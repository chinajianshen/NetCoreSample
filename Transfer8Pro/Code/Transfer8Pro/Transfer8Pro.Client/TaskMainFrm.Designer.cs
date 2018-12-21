namespace Transfer8Pro.Client
{
    partial class TaskMainFrm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.服务管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.任务管理TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.任务列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fTP上传历史FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fTP上传历史列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fTP配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.密钥配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务管理ToolStripMenuItem,
            this.任务管理TToolStripMenuItem,
            this.fTP上传历史FToolStripMenuItem,
            this.配置CToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1233, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 服务管理ToolStripMenuItem
            // 
            this.服务管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置服务ToolStripMenuItem});
            this.服务管理ToolStripMenuItem.Name = "服务管理ToolStripMenuItem";
            this.服务管理ToolStripMenuItem.Size = new System.Drawing.Size(83, 21);
            this.服务管理ToolStripMenuItem.Text = "服务管理(&S)";
            // 
            // 设置服务ToolStripMenuItem
            // 
            this.设置服务ToolStripMenuItem.Name = "设置服务ToolStripMenuItem";
            this.设置服务ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.设置服务ToolStripMenuItem.Text = "设置服务";
            this.设置服务ToolStripMenuItem.Click += new System.EventHandler(this.设置服务ToolStripMenuItem_Click);
            // 
            // 任务管理TToolStripMenuItem
            // 
            this.任务管理TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建任务ToolStripMenuItem,
            this.任务列表ToolStripMenuItem});
            this.任务管理TToolStripMenuItem.Name = "任务管理TToolStripMenuItem";
            this.任务管理TToolStripMenuItem.Size = new System.Drawing.Size(83, 21);
            this.任务管理TToolStripMenuItem.Text = "任务管理(&T)";
            // 
            // 新建任务ToolStripMenuItem
            // 
            this.新建任务ToolStripMenuItem.Name = "新建任务ToolStripMenuItem";
            this.新建任务ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新建任务ToolStripMenuItem.Text = "新建任务";
            this.新建任务ToolStripMenuItem.Click += new System.EventHandler(this.新建任务ToolStripMenuItem_Click);
            // 
            // 任务列表ToolStripMenuItem
            // 
            this.任务列表ToolStripMenuItem.Name = "任务列表ToolStripMenuItem";
            this.任务列表ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.任务列表ToolStripMenuItem.Text = "任务列表";
            this.任务列表ToolStripMenuItem.Click += new System.EventHandler(this.任务列表ToolStripMenuItem_Click);
            // 
            // fTP上传历史FToolStripMenuItem
            // 
            this.fTP上传历史FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fTP上传历史列表ToolStripMenuItem});
            this.fTP上传历史FToolStripMenuItem.Name = "fTP上传历史FToolStripMenuItem";
            this.fTP上传历史FToolStripMenuItem.Size = new System.Drawing.Size(102, 21);
            this.fTP上传历史FToolStripMenuItem.Text = "FTP上传历史(&F)";
            // 
            // fTP上传历史列表ToolStripMenuItem
            // 
            this.fTP上传历史列表ToolStripMenuItem.Name = "fTP上传历史列表ToolStripMenuItem";
            this.fTP上传历史列表ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.fTP上传历史列表ToolStripMenuItem.Text = "FTP上传历史列表";
            this.fTP上传历史列表ToolStripMenuItem.Click += new System.EventHandler(this.fTP上传历史列表ToolStripMenuItem_Click);
            // 
            // 配置CToolStripMenuItem
            // 
            this.配置CToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fTP配置ToolStripMenuItem,
            this.密钥配置ToolStripMenuItem});
            this.配置CToolStripMenuItem.Name = "配置CToolStripMenuItem";
            this.配置CToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.配置CToolStripMenuItem.Text = "配置(&C)";
            // 
            // fTP配置ToolStripMenuItem
            // 
            this.fTP配置ToolStripMenuItem.Name = "fTP配置ToolStripMenuItem";
            this.fTP配置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.fTP配置ToolStripMenuItem.Text = "FTP配置";
            this.fTP配置ToolStripMenuItem.Click += new System.EventHandler(this.fTP配置ToolStripMenuItem_Click);
            // 
            // 密钥配置ToolStripMenuItem
            // 
            this.密钥配置ToolStripMenuItem.Name = "密钥配置ToolStripMenuItem";
            this.密钥配置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.密钥配置ToolStripMenuItem.Text = "密钥配置";
            this.密钥配置ToolStripMenuItem.Click += new System.EventHandler(this.密钥配置ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 732);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1233, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TaskMainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 754);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TaskMainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "传8客户端";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TaskMainFrm_FormClosed);
            this.Load += new System.EventHandler(this.TaskMainFrm_Load);
            this.Shown += new System.EventHandler(this.TaskMainFrm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem 服务管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 任务管理TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 配置CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fTP上传历史FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fTP配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 密钥配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 任务列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fTP上传历史列表ToolStripMenuItem;
    }
}