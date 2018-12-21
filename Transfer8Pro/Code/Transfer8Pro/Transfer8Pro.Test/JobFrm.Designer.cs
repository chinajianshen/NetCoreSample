namespace Transfer8Pro.Test
{
    partial class JobFrm
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
            this.txtTaskID = new System.Windows.Forms.TextBox();
            this.btnJobStart = new System.Windows.Forms.Button();
            this.btnJobStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务ID";
            // 
            // txtTaskID
            // 
            this.txtTaskID.Location = new System.Drawing.Point(104, 30);
            this.txtTaskID.Name = "txtTaskID";
            this.txtTaskID.Size = new System.Drawing.Size(428, 21);
            this.txtTaskID.TabIndex = 1;
            this.txtTaskID.Text = "492783795ea2470f9a80ace8c4ca78f7";
            // 
            // btnJobStart
            // 
            this.btnJobStart.Location = new System.Drawing.Point(104, 83);
            this.btnJobStart.Name = "btnJobStart";
            this.btnJobStart.Size = new System.Drawing.Size(136, 31);
            this.btnJobStart.TabIndex = 2;
            this.btnJobStart.Text = "执行作业";
            this.btnJobStart.UseVisualStyleBackColor = true;
            this.btnJobStart.Click += new System.EventHandler(this.btnJobStart_Click);
            // 
            // btnJobStop
            // 
            this.btnJobStop.Location = new System.Drawing.Point(302, 83);
            this.btnJobStop.Name = "btnJobStop";
            this.btnJobStop.Size = new System.Drawing.Size(136, 31);
            this.btnJobStop.TabIndex = 3;
            this.btnJobStop.Text = "结束作业";
            this.btnJobStop.UseVisualStyleBackColor = true;
            this.btnJobStop.Click += new System.EventHandler(this.btnJobStop_Click);
            // 
            // JobFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 464);
            this.Controls.Add(this.btnJobStop);
            this.Controls.Add(this.btnJobStart);
            this.Controls.Add(this.txtTaskID);
            this.Controls.Add(this.label1);
            this.Name = "JobFrm";
            this.Text = "JobFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaskID;
        private System.Windows.Forms.Button btnJobStart;
        private System.Windows.Forms.Button btnJobStop;
    }
}