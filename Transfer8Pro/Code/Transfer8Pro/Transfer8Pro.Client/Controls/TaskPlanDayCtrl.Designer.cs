namespace Transfer8Pro.Client.Controls
{
    partial class TaskPlanDayCtrl
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
            this.rdoOneTimes = new System.Windows.Forms.RadioButton();
            this.rdoMultiTimes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxOneTimesHour = new System.Windows.Forms.ComboBox();
            this.cbxOneTimesMinute = new System.Windows.Forms.ComboBox();
            this.cbxMultiTimesMinute = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lstReecntTimes = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rdoOneTimes
            // 
            this.rdoOneTimes.AutoSize = true;
            this.rdoOneTimes.Location = new System.Drawing.Point(22, 28);
            this.rdoOneTimes.Name = "rdoOneTimes";
            this.rdoOneTimes.Size = new System.Drawing.Size(71, 16);
            this.rdoOneTimes.TabIndex = 0;
            this.rdoOneTimes.Text = "执行一次";
            this.rdoOneTimes.UseVisualStyleBackColor = true;
            this.rdoOneTimes.CheckedChanged += new System.EventHandler(this.rdoOneTimes_CheckedChanged);
            // 
            // rdoMultiTimes
            // 
            this.rdoMultiTimes.AutoSize = true;
            this.rdoMultiTimes.Location = new System.Drawing.Point(22, 73);
            this.rdoMultiTimes.Name = "rdoMultiTimes";
            this.rdoMultiTimes.Size = new System.Drawing.Size(71, 16);
            this.rdoMultiTimes.TabIndex = 1;
            this.rdoMultiTimes.Text = "执行多次";
            this.rdoMultiTimes.UseVisualStyleBackColor = true;
            this.rdoMultiTimes.CheckedChanged += new System.EventHandler(this.rdoMultiTimes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "执行时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "每隔";
            // 
            // cbxOneTimesHour
            // 
            this.cbxOneTimesHour.FormattingEnabled = true;
            this.cbxOneTimesHour.Location = new System.Drawing.Point(165, 28);
            this.cbxOneTimesHour.Name = "cbxOneTimesHour";
            this.cbxOneTimesHour.Size = new System.Drawing.Size(44, 20);
            this.cbxOneTimesHour.TabIndex = 4;
            this.cbxOneTimesHour.SelectedIndexChanged += new System.EventHandler(this.cbxOneTimesHour_SelectedIndexChanged);
            // 
            // cbxOneTimesMinute
            // 
            this.cbxOneTimesMinute.FormattingEnabled = true;
            this.cbxOneTimesMinute.Location = new System.Drawing.Point(238, 28);
            this.cbxOneTimesMinute.Name = "cbxOneTimesMinute";
            this.cbxOneTimesMinute.Size = new System.Drawing.Size(46, 20);
            this.cbxOneTimesMinute.TabIndex = 5;
            this.cbxOneTimesMinute.SelectedIndexChanged += new System.EventHandler(this.cbxOneTimesMinute_SelectedIndexChanged);
            // 
            // cbxMultiTimesMinute
            // 
            this.cbxMultiTimesMinute.FormattingEnabled = true;
            this.cbxMultiTimesMinute.Location = new System.Drawing.Point(165, 69);
            this.cbxMultiTimesMinute.Name = "cbxMultiTimesMinute";
            this.cbxMultiTimesMinute.Size = new System.Drawing.Size(53, 20);
            this.cbxMultiTimesMinute.TabIndex = 6;
            this.cbxMultiTimesMinute.SelectedIndexChanged += new System.EventHandler(this.cbxMultiTimesMinute_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "时";
            this.label4.UseWaitCursor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(287, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "分";
            this.label5.UseWaitCursor = true;
            // 
            // lstReecntTimes
            // 
            this.lstReecntTimes.FormattingEnabled = true;
            this.lstReecntTimes.ItemHeight = 12;
            this.lstReecntTimes.Location = new System.Drawing.Point(468, 28);
            this.lstReecntTimes.Name = "lstReecntTimes";
            this.lstReecntTimes.Size = new System.Drawing.Size(258, 100);
            this.lstReecntTimes.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(362, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "最近8次执行时间";
            // 
            // TaskPlanDayCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lstReecntTimes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbxMultiTimesMinute);
            this.Controls.Add(this.cbxOneTimesMinute);
            this.Controls.Add(this.cbxOneTimesHour);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoMultiTimes);
            this.Controls.Add(this.rdoOneTimes);
            this.Name = "TaskPlanDayCtrl";
            this.Size = new System.Drawing.Size(898, 144);
            this.Load += new System.EventHandler(this.TaskPlanDayCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoOneTimes;
        private System.Windows.Forms.RadioButton rdoMultiTimes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxOneTimesHour;
        private System.Windows.Forms.ComboBox cbxOneTimesMinute;
        private System.Windows.Forms.ComboBox cbxMultiTimesMinute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstReecntTimes;
        private System.Windows.Forms.Label label6;
    }
}
