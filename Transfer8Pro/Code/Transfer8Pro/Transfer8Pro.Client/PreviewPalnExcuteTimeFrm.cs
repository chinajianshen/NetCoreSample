using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Core;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client
{
    public partial class PreviewPalnExcuteTimeFrm : BaseFrm
    {
        public PreviewPalnExcuteTimeFrm()
        {
            InitializeComponent();
        }

        string _TaskName;
        string _CronString;

        public PreviewPalnExcuteTimeFrm(string taskName,string cronString) : this()
        {
            _TaskName = taskName;
            _CronString = cronString;
        }

        private void PreviewPalnExcuteTimeFrm_Load(object sender, EventArgs e)
        {
            try
            {
                lblTaskName.Text = _TaskName;
                List<DateTime> dateTimeList = QuartzHelper.GetNextFireTime(_CronString, 50);
                List<KVEntity> list = dateTimeList.Select(item =>  new KVEntity { K = item.ToString("yyyy-MM-dd HH:mm:ss")}).ToList();
                BindControl.BindListBox(lstList, list);
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"Cron表达解析错误，错误信息：[{ex.Message}]");
            }
           
        }
    }
}
