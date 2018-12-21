namespace Transfer8Pro.Client.Controls
{
    partial class TaskPlanMonthCtrl
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
            this.label4 = new System.Windows.Forms.Label();
            this.lstRecentTimes = new System.Windows.Forms.ListBox();
            this.chklstMonths = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxMinute = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxHour = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(666, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "最近8次执行时间";
            // 
            // lstRecentTimes
            // 
            this.lstRecentTimes.FormattingEnabled = true;
            this.lstRecentTimes.ItemHeight = 12;
            this.lstRecentTimes.Location = new System.Drawing.Point(664, 36);
            this.lstRecentTimes.Name = "lstRecentTimes";
            this.lstRecentTimes.Size = new System.Drawing.Size(218, 100);
            this.lstRecentTimes.TabIndex = 8;
            // 
            // chklstMonths
            // 
            this.chklstMonths.BackColor = System.Drawing.SystemColors.Control;
            this.chklstMonths.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstMonths.CheckOnClick = true;
            this.chklstMonths.ColumnWidth = 90;
            this.chklstMonths.Location = new System.Drawing.Point(13, 16);
            this.chklstMonths.MultiColumn = true;
            this.chklstMonths.Name = "chklstMonths";
            this.chklstMonths.Size = new System.Drawing.Size(634, 80);
            this.chklstMonths.TabIndex = 10;
            this.chklstMonths.SelectedIndexChanged += new System.EventHandler(this.chklstMonths_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "分";
            // 
            // cbxMinute
            // 
            this.cbxMinute.FormattingEnabled = true;
            this.cbxMinute.Location = new System.Drawing.Point(137, 109);
            this.cbxMinute.Name = "cbxMinute";
            this.cbxMinute.Size = new System.Drawing.Size(41, 20);
            this.cbxMinute.TabIndex = 14;
            this.cbxMinute.SelectedIndexChanged += new System.EventHandler(this.cbxMinute_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "时";
            // 
            // cbxHour
            // 
            this.cbxHour.FormattingEnabled = true;
            this.cbxHour.Location = new System.Drawing.Point(68, 109);
            this.cbxHour.Name = "cbxHour";
            this.cbxHour.Size = new System.Drawing.Size(41, 20);
            this.cbxHour.TabIndex = 12;
            this.cbxHour.SelectedIndexChanged += new System.EventHandler(this.cbxHour_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "执行时间";
            // 
            // TaskPlanMonthCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxMinute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxHour);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chklstMonths);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstRecentTimes);
            this.Name = "TaskPlanMonthCtrl";
            this.Size = new System.Drawing.Size(898, 144);
            this.Load += new System.EventHandler(this.TaskPlanMonthCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstRecentTimes;
        private System.Windows.Forms.CheckedListBox chklstMonths;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxMinute;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxHour;
        private System.Windows.Forms.Label label1;
    }
}
