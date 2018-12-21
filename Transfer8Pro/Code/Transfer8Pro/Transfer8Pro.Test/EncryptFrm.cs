using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Test
{
    public partial class EncryptFrm : Form
    {
        public EncryptFrm()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtString.Text))
            {
                MessageBox.Show("字符串不能为空");
                return;
            }

            if (string.IsNullOrEmpty(txtSecretKey.Text))
            {
                MessageBox.Show("密钥不能为空");
                return;
            }

           string encryptString =  RijndaelCrypt.Encrypt(rtxtString.Text.Trim(), txtSecretKey.Text.Trim());
            rtxtEncryString.Text = encryptString;
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtEncryString.Text))
            {
                MessageBox.Show("加密串不能为空");
                return;
            }

            if (string.IsNullOrEmpty(txtSecretKey.Text))
            {
                MessageBox.Show("密钥不能为空");
                return;
            }

            string str = RijndaelCrypt.Decrypt(rtxtEncryString.Text.Trim(), txtSecretKey.Text.Trim());
            rtxtString.Text = str;
        }
    }
}
