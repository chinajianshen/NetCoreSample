namespace Transfer8Pro.Test
{
    partial class EncryptFrm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.rtxtString = new System.Windows.Forms.RichTextBox();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxtEncryString = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDecode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(74, 306);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(110, 34);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "加密";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // rtxtString
            // 
            this.rtxtString.Location = new System.Drawing.Point(74, 23);
            this.rtxtString.Name = "rtxtString";
            this.rtxtString.Size = new System.Drawing.Size(529, 89);
            this.rtxtString.TabIndex = 1;
            this.rtxtString.Text = "  server=192.168.9.138\\SMART;database=Smart_NewBookDB;uid=sa;pwd=sa.;min pool siz" +
    "e=10;max pool size=300;Connection Timeout=10;";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(74, 256);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(529, 21);
            this.txtSecretKey.TabIndex = 2;
            this.txtSecretKey.Text = "08491090B30349FB89C543A477946459";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "字符串";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "密钥";
            // 
            // rtxtEncryString
            // 
            this.rtxtEncryString.Location = new System.Drawing.Point(74, 144);
            this.rtxtEncryString.Name = "rtxtEncryString";
            this.rtxtEncryString.Size = new System.Drawing.Size(529, 89);
            this.rtxtEncryString.TabIndex = 5;
            this.rtxtEncryString.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "加密串";
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(223, 306);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(110, 34);
            this.btnDecode.TabIndex = 7;
            this.btnDecode.Text = "解密";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 457);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rtxtEncryString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSecretKey);
            this.Controls.Add(this.rtxtString);
            this.Controls.Add(this.btnEncrypt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.RichTextBox rtxtString;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxtEncryString;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDecode;
    }
}

