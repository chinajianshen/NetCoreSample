using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Core;
using Transfer8Pro.Entity;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core;

namespace Transfer8Pro.Client.Controls
{
    public partial class TaskPlanWeekCtrl : UserControl, ITaskPlanBase
    {
        CronExpressionEntity _cronExpressionEntity = null;
        public TaskPlanWeekCtrl()
        {
            InitializeComponent();
        }

        private void TaskPlanWeekCtrl_Load(object sender, EventArgs e)
        {
            BindData(_cronExpressionEntity);
        }

        public CronExpressionEntity CronExpressionEntity
        {
            get
            {
                return _cronExpressionEntity;
            }
            set
            {
                _cronExpressionEntity = value;

            }
        }

        public string GetCronExpressionString()
        {
            return QuartzHelper.GenerateCronExpression(_cronExpressionEntity);
        }

        public bool ValidateData()
        {
            if (chklstWeeks.CheckedItems.Count == 0)
            {
                MessageBox.Show("请至少选择一个星期项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chklstWeeks.Focus();
                return false;
            }

            if (cbxHour.SelectedIndex == 0)
            {
                MessageBox.Show("请选择计划执行开始时间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxHour.Focus();
                return false;
            }

            if (cbxMinute.SelectedIndex == 0)
            {
                MessageBox.Show("请选择计划执行开始分钟", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxMinute.Focus();
                return false;
            }
            return true;
        }

        private void BindData(CronExpressionEntity cronExp)
        {

            //绑定执行一次 小时下拉列表
            BindControl.BindComboBox(cbxHour, Common.DigitalHours(),cronExp != null ? cronExp.Hour:"");
            //绑定执行一次 分下拉列表
            BindControl.BindComboBox(cbxMinute, Common.DigitalMinutes(),cronExp != null ? cronExp.Minute:"");
            //绑定复选框组
            BindControl.BindCheckedListBox(chklstWeeks, Common.DigitalWeeks(),cronExp != null ? cronExp.SelectedTimestamp:"");  
            
            if (cronExp != null && chklstWeeks.CheckedItems.Count>0)
            {
                BindWeeksToListBox();
            }
        }

        private void cbxHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cronExpressionEntity == null)
            {
                _cronExpressionEntity = new CronExpressionEntity();
                _cronExpressionEntity.CycleTypes = CycleTypes.W;

            }

            if (cbxHour.SelectedIndex != 0)
            {
                _cronExpressionEntity.Hour = cbxHour.SelectedValue.ToString();
            }

            BindlstReecntTimes();
        }

        private void cbxMinute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cronExpressionEntity == null)
            {
                _cronExpressionEntity = new CronExpressionEntity();
                _cronExpressionEntity.CycleTypes = CycleTypes.W;
            }

            if (cbxMinute.SelectedIndex != 0)
            {
                _cronExpressionEntity.Minute = cbxMinute.SelectedValue.ToString();
            }

            BindlstReecntTimes();
        }

        private void chklstWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cronExpressionEntity == null)
            {
                _cronExpressionEntity = new CronExpressionEntity();
                _cronExpressionEntity.CycleTypes = CycleTypes.W;
            }

            BindWeeksToListBox();
        }

        private void BindWeeksToListBox()
        {
            List<string> weeks = new List<string>();

            for (int index = 0; index < chklstWeeks.CheckedItems.Count; index++)
            {
                KVEntity kv = chklstWeeks.CheckedItems[index] as KVEntity;
                weeks.Add(kv.V);
            }

            if (weeks.Count > 0)
            {
                _cronExpressionEntity.SelectedTimestamp = string.Join(",", weeks);
            }
            else
            {
                _cronExpressionEntity.SelectedTimestamp = "";
            }

            BindlstReecntTimes();
        }

        private void BindlstReecntTimes()
        {
            BindControl.BindListBox(lstRecentTimes, new List<KVEntity>());
            if (cbxHour.SelectedIndex == 0 || cbxMinute.SelectedIndex == 0 || chklstWeeks.CheckedItems.Count == 0)
            {
                return;
            }

            string cronString = QuartzHelper.GenerateCronExpression(_cronExpressionEntity);
            List<DateTime> list = QuartzHelper.GetNextFireTime(cronString, 8);
            List<KVEntity> kvList = list.Select(item => new KVEntity { K = item.ToString(), V = item.ToString() }).ToList();
            BindControl.BindListBox(lstRecentTimes, kvList);
        }
    }
}
