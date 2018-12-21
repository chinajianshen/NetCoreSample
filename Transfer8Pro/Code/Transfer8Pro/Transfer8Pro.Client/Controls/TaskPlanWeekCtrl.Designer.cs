namespace Transfer8Pro.Client.Controls
{
    partial class TaskPlanWeekCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chklstWeeks = new System.Windows.Forms.CheckedListBox();
            this.lstRecentTimes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxHour = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxMinute = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chklstWeeks
            // 
            this.chklstWeeks.BackColor = System.Drawing.SystemColors.Control;
            this.chklstWeeks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstWeeks.CheckOnClick = true;
            this.chklstWeeks.ColumnWidth = 80;
            this.chklstWeeks.Location = new System.Drawing.Point(12, 13);
            this.chklstWeeks.MultiColumn = true;
            this.chklstWeeks.Name = "chklstWeeks";
            this.chklstWeeks.Size = new System.Drawing.Size(586, 16);
            this.chklstWeeks.TabIndex = 0;
            this.chklstWeeks.SelectedIndexChanged += new System.EventHandler(this.chklstWeeks_SelectedIndexChanged);
            // 
            // lstRecentTimes
            // 
            this.lstRecentTimes.FormattingEnabled = true;
            this.lstRecentTimes.ItemHeight = 12;
            this.lstRecentTimes.Location = new System.Drawing.Point(619, 33);
            this.lstRecentTimes.Name = "lstRecentTimes";
            this.lstRecentTimes.Size = new System.Drawing.Size(258, 100);
            this.lstRecentTimes.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "执行时间";
            // 
            // cbxHour
            // 
            this.cbxHour.FormattingEnabled = true;
            this.cbxHour.Location = new System.Drawing.Point(69, 58);
            this.cbxHour.Name = "cbxHour";
            this.cbxHour.Size = new System.Drawing.Size(41, 20);
            this.cbxHour.TabIndex = 3;
            this.cbxHour.SelectedIndexChanged += new System.EventHandler(this.cbxHour_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "时";
            // 
            // cbxMinute
            // 
            this.cbxMinute.FormattingEnabled = true;
            this.cbxMinute.Location = new System.Drawing.Point(138, 58);
            this.cbxMinute.Name = "cbxMinute";
            this.cbxMinute.Size = new System.Drawing.Size(41, 20);
            this.cbxMinute.TabIndex = 5;
            this.cbxMinute.SelectedIndexChanged += new System.EventHandler(this.cbxMinute_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "分";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(617, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "最近8次执行时间";
            // 
            // TaskPlanWeekCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxMinute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxHour);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstRecentTimes);
            this.Controls.Add(this.chklstWeeks);
            this.Name = "TaskPlanWeekCtrl";
            this.Size = new System.Drawing.Size(898, 144);
            this.Load += new System.EventHandler(this.TaskPlanWeekCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chklstWeeks;
        private System.Windows.Forms.ListBox lstRecentTimes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxHour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxMinute;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
