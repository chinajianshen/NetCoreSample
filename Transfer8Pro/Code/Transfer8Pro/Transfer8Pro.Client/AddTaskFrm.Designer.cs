namespace Transfer8Pro.Client
{
    partial class AddTaskFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDbCheck = new System.Windows.Forms.Button();
            this.cbxDataBaseType = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlPlan = new System.Windows.Forms.Panel();
            this.rdoMonth = new System.Windows.Forms.RadioButton();
            this.rdoWeek = new System.Windows.Forms.RadioButton();
            this.rdoDay = new System.Windows.Forms.RadioButton();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDbConn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxDataTypes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnDbCheck);
            this.panel1.Controls.Add(this.cbxDataBaseType);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.txtSql);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtDbConn);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbxDataTypes);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtTaskName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 667);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(304, 615);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(129, 49);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDbCheck
            // 
            this.btnDbCheck.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnDbCheck.FlatAppearance.BorderSize = 0;
            this.btnDbCheck.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDbCheck.ForeColor = System.Drawing.Color.White;
            this.btnDbCheck.Location = new System.Drawing.Point(823, 358);
            this.btnDbCheck.Name = "btnDbCheck";
            this.btnDbCheck.Size = new System.Drawing.Size(97, 37);
            this.btnDbCheck.TabIndex = 16;
            this.btnDbCheck.Text = "检测";
            this.btnDbCheck.UseVisualStyleBackColor = false;
            this.btnDbCheck.Click += new System.EventHandler(this.btnDbCheck_Click);
            // 
            // cbxDataBaseType
            // 
            this.cbxDataBaseType.FormattingEnabled = true;
            this.cbxDataBaseType.Location = new System.Drawing.Point(89, 54);
            this.cbxDataBaseType.Name = "cbxDataBaseType";
            this.cbxDataBaseType.Size = new System.Drawing.Size(415, 20);
            this.cbxDataBaseType.TabIndex = 15;
            this.cbxDataBaseType.SelectedIndexChanged += new System.EventHandler(this.cbxDataBaseType_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(439, 615);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(129, 49);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pnlPlan);
            this.groupBox1.Controls.Add(this.rdoMonth);
            this.groupBox1.Controls.Add(this.rdoWeek);
            this.groupBox1.Controls.Add(this.rdoDay);
            this.groupBox1.Location = new System.Drawing.Point(14, 404);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(906, 205);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执行计划";
            // 
            // pnlPlan
            // 
            this.pnlPlan.Location = new System.Drawing.Point(7, 49);
            this.pnlPlan.Name = "pnlPlan";
            this.pnlPlan.Size = new System.Drawing.Size(913, 146);
            this.pnlPlan.TabIndex = 3;
            // 
            // rdoMonth
            // 
            this.rdoMonth.AutoSize = true;
            this.rdoMonth.Location = new System.Drawing.Point(144, 27);
            this.rdoMonth.Name = "rdoMonth";
            this.rdoMonth.Size = new System.Drawing.Size(47, 16);
            this.rdoMonth.TabIndex = 2;
            this.rdoMonth.Text = "每月";
            this.rdoMonth.UseVisualStyleBackColor = true;
            this.rdoMonth.CheckedChanged += new System.EventHandler(this.rdoMonth_CheckedChanged);
            // 
            // rdoWeek
            // 
            this.rdoWeek.AutoSize = true;
            this.rdoWeek.Location = new System.Drawing.Point(73, 27);
            this.rdoWeek.Name = "rdoWeek";
            this.rdoWeek.Size = new System.Drawing.Size(47, 16);
            this.rdoWeek.TabIndex = 1;
            this.rdoWeek.Text = "每周";
            this.rdoWeek.UseVisualStyleBackColor = true;
            this.rdoWeek.CheckedChanged += new System.EventHandler(this.rdoWeek_CheckedChanged);
            // 
            // rdoDay
            // 
            this.rdoDay.AutoSize = true;
            this.rdoDay.Location = new System.Drawing.Point(11, 27);
            this.rdoDay.Name = "rdoDay";
            this.rdoDay.Size = new System.Drawing.Size(47, 16);
            this.rdoDay.TabIndex = 0;
            this.rdoDay.Text = "每天";
            this.rdoDay.UseVisualStyleBackColor = true;
            this.rdoDay.CheckedChanged += new System.EventHandler(this.rdoDay_CheckedChanged);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(14, 243);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSql.Size = new System.Drawing.Size(906, 102);
            this.txtSql.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "SQL语句配置";
            // 
            // txtDbConn
            // 
            this.txtDbConn.Location = new System.Drawing.Point(14, 118);
            this.txtDbConn.Multiline = true;
            this.txtDbConn.Name = "txtDbConn";
            this.txtDbConn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDbConn.Size = new System.Drawing.Size(906, 78);
            this.txtDbConn.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "数据库连接字符串配置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "数据库类型：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(511, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "*";
            // 
            // cbxDataTypes
            // 
            this.cbxDataTypes.FormattingEnabled = true;
            this.cbxDataTypes.Location = new System.Drawing.Point(652, 17);
            this.cbxDataTypes.Name = "cbxDataTypes";
            this.cbxDataTypes.Size = new System.Drawing.Size(268, 20);
            this.cbxDataTypes.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(591, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据类型：";
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(89, 17);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(415, 21);
            this.txtTaskName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务名称：";
            // 
            // AddTaskFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 667);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "AddTaskFrm";
            this.Text = "新建任务";
            this.Load += new System.EventHandler(this.AddTaskFrm_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoMonth;
        private System.Windows.Forms.RadioButton rdoWeek;
        private System.Windows.Forms.RadioButton rdoDay;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDbConn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxDataTypes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlPlan;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cbxDataBaseType;
        private System.Windows.Forms.Button btnDbCheck;
        private System.Windows.Forms.Button btnClose;
    }
}