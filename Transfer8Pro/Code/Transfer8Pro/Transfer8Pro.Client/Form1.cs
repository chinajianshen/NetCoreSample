using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Core.Service;

namespace Transfer8Pro.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }
        TestTaskService ts = new TestTaskService();
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ts.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            ts.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestService ts = new TestService();
            ts.Test_Oracle();
            MessageBox.Show("ok");
        }
    }
}
