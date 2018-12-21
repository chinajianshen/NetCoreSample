namespace Transfer8Pro.Client
{
    partial class TaskListFrm
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbxEnabledStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxTaskStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxCycleType = new System.Windows.Forms.ComboBox();
            this.cbxDataType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTaskID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvTaskList = new System.Windows.Forms.DataGridView();
            this.TaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CycleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cron = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecentRunTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextFireTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pagerCtrl1 = new Transfer8Pro.Client.Controls.PagerCtrl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制任务IDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看执行计划时间ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看执行历史ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskList)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1207, 631);
            this.splitContainer1.SplitterDistance = 111;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreate);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.cbxEnabledStatus);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbxTaskStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbxCycleType);
            this.groupBox1.Controls.Add(this.cbxDataType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtTaskName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTaskID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1207, 111);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(1049, 23);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 14;
            this.btnCreate.Text = "新建";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(957, 70);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbxEnabledStatus
            // 
            this.cbxEnabledStatus.FormattingEnabled = true;
            this.cbxEnabledStatus.Location = new System.Drawing.Point(678, 69);
            this.cbxEnabledStatus.Name = "cbxEnabledStatus";
            this.cbxEnabledStatus.Size = new System.Drawing.Size(193, 20);
            this.cbxEnabledStatus.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(619, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "启用状态";
            // 
            // cbxTaskStatus
            // 
            this.cbxTaskStatus.FormattingEnabled = true;
            this.cbxTaskStatus.Location = new System.Drawing.Point(678, 26);
            this.cbxTaskStatus.Name = "cbxTaskStatus";
            this.cbxTaskStatus.Size = new System.Drawing.Size(193, 20);
            this.cbxTaskStatus.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(619, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "任务状态";
            // 
            // cbxCycleType
            // 
            this.cbxCycleType.FormattingEnabled = true;
            this.cbxCycleType.Location = new System.Drawing.Point(392, 71);
            this.cbxCycleType.Name = "cbxCycleType";
            this.cbxCycleType.Size = new System.Drawing.Size(193, 20);
            this.cbxCycleType.TabIndex = 8;
            // 
            // cbxDataType
            // 
            this.cbxDataType.FormattingEnabled = true;
            this.cbxDataType.Location = new System.Drawing.Point(392, 29);
            this.cbxDataType.Name = "cbxDataType";
            this.cbxDataType.Size = new System.Drawing.Size(193, 20);
            this.cbxDataType.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(332, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "时间类型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(332, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "数据类型";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(956, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(71, 71);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(238, 21);
            this.txtTaskName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "任务名称";
            // 
            // txtTaskID
            // 
            this.txtTaskID.Location = new System.Drawing.Point(71, 29);
            this.txtTaskID.Name = "txtTaskID";
            this.txtTaskID.Size = new System.Drawing.Size(238, 21);
            this.txtTaskID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "TaskID";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvTaskList);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pagerCtrl1);
            this.splitContainer2.Size = new System.Drawing.Size(1207, 516);
            this.splitContainer2.SplitterDistance = 468;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgvTaskList
            // 
            this.dgvTaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaskID,
            this.TaskName,
            this.DataType,
            this.CycleType,
            this.Cron,
            this.TaskStatus,
            this.RecentRunTime,
            this.NextFireTime,
            this.Enabled});
            this.dgvTaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTaskList.Location = new System.Drawing.Point(0, 0);
            this.dgvTaskList.Name = "dgvTaskList";
            this.dgvTaskList.RowTemplate.Height = 23;
            this.dgvTaskList.Size = new System.Drawing.Size(1207, 468);
            this.dgvTaskList.TabIndex = 0;
            // 
            // TaskID
            // 
            this.TaskID.HeaderText = "TaskID";
            this.TaskID.Name = "TaskID";
            this.TaskID.Width = 300;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "任务名称";
            this.TaskName.Name = "TaskName";
            this.TaskName.Width = 200;
            // 
            // DataType
            // 
            this.DataType.HeaderText = "数据类型";
            this.DataType.Name = "DataType";
            // 
            // CycleType
            // 
            this.CycleType.HeaderText = "时间类型";
            this.CycleType.Name = "CycleType";
            // 
            // Cron
            // 
            this.Cron.HeaderText = "执行周期表达式";
            this.Cron.Name = "Cron";
            this.Cron.Width = 150;
            // 
            // TaskStatus
            // 
            this.TaskStatus.HeaderText = "状态";
            this.TaskStatus.Name = "TaskStatus";
            // 
            // RecentRunTime
            // 
            this.RecentRunTime.HeaderText = "最近一次执行时间";
            this.RecentRunTime.Name = "RecentRunTime";
            this.RecentRunTime.Width = 130;
            // 
            // NextFireTime
            // 
            this.NextFireTime.HeaderText = "下次执行时间";
            this.NextFireTime.Name = "NextFireTime";
            this.NextFireTime.Width = 125;
            // 
            // Enabled
            // 
            this.Enabled.HeaderText = "启用状态";
            this.Enabled.Name = "Enabled";
            this.Enabled.Width = 50;
            // 
            // pagerCtrl1
            // 
            this.pagerCtrl1.CurrentPageNo = 0;
            this.pagerCtrl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pagerCtrl1.Location = new System.Drawing.Point(721, 0);
            this.pagerCtrl1.Name = "pagerCtrl1";
            this.pagerCtrl1.PageSize = 20;
            this.pagerCtrl1.Size = new System.Drawing.Size(486, 44);
            this.pagerCtrl1.TabIndex = 0;
            this.pagerCtrl1.TotalPages = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改任务ToolStripMenuItem,
            this.查看任务ToolStripMenuItem,
            this.停用ToolStripMenuItem,
            this.复制任务IDToolStripMenuItem,
            this.查看执行计划时间ToolStripMenuItem,
            this.查看执行历史ToolStripMenuItem,
            this.删除任务ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 158);
            // 
            // 修改任务ToolStripMenuItem
            // 
            this.修改任务ToolStripMenuItem.Name = "修改任务ToolStripMenuItem";
            this.修改任务ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.修改任务ToolStripMenuItem.Text = "修改任务";
            this.修改任务ToolStripMenuItem.Click += new System.EventHandler(this.修改任务ToolStripMenuItem_Click);
            // 
            // 查看任务ToolStripMenuItem
            // 
            this.查看任务ToolStripMenuItem.Name = "查看任务ToolStripMenuItem";
            this.查看任务ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看任务ToolStripMenuItem.Text = "查看任务";
            this.查看任务ToolStripMenuItem.Click += new System.EventHandler(this.查看任务ToolStripMenuItem_Click);
            // 
            // 停用ToolStripMenuItem
            // 
            this.停用ToolStripMenuItem.Name = "停用ToolStripMenuItem";
            this.停用ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.停用ToolStripMenuItem.Text = "停用任务";
            this.停用ToolStripMenuItem.Click += new System.EventHandler(this.停用ToolStripMenuItem_Click);
            // 
            // 复制任务IDToolStripMenuItem
            // 
            this.复制任务IDToolStripMenuItem.Name = "复制任务IDToolStripMenuItem";
            this.复制任务IDToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.复制任务IDToolStripMenuItem.Text = "复制任务ID";
            this.复制任务IDToolStripMenuItem.Click += new System.EventHandler(this.复制任务IDToolStripMenuItem_Click);
            // 
            // 查看执行计划时间ToolStripMenuItem
            // 
            this.查看执行计划时间ToolStripMenuItem.Name = "查看执行计划时间ToolStripMenuItem";
            this.查看执行计划时间ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看执行计划时间ToolStripMenuItem.Text = "查看执行计划时间";
            this.查看执行计划时间ToolStripMenuItem.Click += new System.EventHandler(this.查看执行计划时间ToolStripMenuItem_Click);
            // 
            // 查看执行历史ToolStripMenuItem
            // 
            this.查看执行历史ToolStripMenuItem.Name = "查看执行历史ToolStripMenuItem";
            this.查看执行历史ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看执行历史ToolStripMenuItem.Text = "查看执行历史";
            this.查看执行历史ToolStripMenuItem.Click += new System.EventHandler(this.查看执行历史ToolStripMenuItem_Click);
            // 
            // 删除任务ToolStripMenuItem
            // 
            this.删除任务ToolStripMenuItem.Name = "删除任务ToolStripMenuItem";
            this.删除任务ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.删除任务ToolStripMenuItem.Text = "删除任务";
            this.删除任务ToolStripMenuItem.Click += new System.EventHandler(this.删除任务ToolStripMenuItem_Click);
            // 
            // TaskListFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 631);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TaskListFrm";
            this.Text = "任务列表";
            this.Load += new System.EventHandler(this.TaskListFrm_Load);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskList)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvTaskList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 修改任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制任务IDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看执行计划时间ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看执行历史ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看任务ToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbxCycleType;
        private System.Windows.Forms.ComboBox cbxDataType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTaskID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxTaskStatus;
        private System.Windows.Forms.Label label5;
        private Controls.PagerCtrl pagerCtrl1;
        private System.Windows.Forms.ComboBox cbxEnabledStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CycleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cron;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecentRunTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextFireTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Enabled;
        private System.Windows.Forms.ToolStripMenuItem 停用ToolStripMenuItem;
        private System.Windows.Forms.Button btnCreate;
    }
}