using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Transfer8Pro.Client
{
    public partial class TaskHistoryListFrm : Form
    {
        private string _TaskID;
        public TaskHistoryListFrm()
        {
            InitializeComponent();
        }

        public TaskHistoryListFrm(string taskID):this()
        {
            _TaskID = taskID;
        }
    }
}
