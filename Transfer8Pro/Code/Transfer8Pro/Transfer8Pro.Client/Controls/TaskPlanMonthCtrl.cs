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
    public partial class TaskPlanMonthCtrl : UserControl, ITaskPlanBase
    {
        CronExpressionEntity _cronExpressionEntity = null;
        public TaskPlanMonthCtrl()
        {
            InitializeComponent();
        }

        private void TaskPlanMonthCtrl_Load(object sender, EventArgs e)
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
            if (chklstMonths.CheckedItems.Count == 0)
            {
                MessageBox.Show("请至少选择一个月份项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chklstMonths.Focus();
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
            BindControl.BindComboBox(cbxHour, Common.DigitalHours(), cronExp != null ? cronExp.Hour :"");
            //绑定执行一次 分下拉列表
            BindControl.BindComboBox(cbxMinute, Common.DigitalMinutes(), cronExp != null ? cronExp.Minute:"");
            //绑定复选框组
            BindControl.BindCheckedListBox(chklstMonths, Common.DigitalMonths(), cronExp != null ? cronExp.SelectedTimestamp:"");        
            
            if (cronExp != null && chklstMonths.CheckedItems.Count > 0)
            {
                BindMonthsToListBox();
            }
        }

        private void chklstMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cronExpressionEntity == null)
            {
                _cronExpressionEntity = new CronExpressionEntity();
                _cronExpressionEntity.CycleTypes = CycleTypes.M;
            }

            BindMonthsToListBox();
        }

        private void BindMonthsToListBox()
        {
            List<string> months = new List<string>();

            for (int index = 0; index < chklstMonths.CheckedItems.Count; index++)
            {
                KVEntity kv = chklstMonths.CheckedItems[index] as KVEntity;
                months.Add(kv.V);
            }

            if (months.Count > 0)
            {
                _cronExpressionEntity.SelectedTimestamp = string.Join(",", months);
            }
            else
            {
                _cronExpressionEntity.SelectedTimestamp = "";
            }

            BindlstReecntTimes();
        }

        private void cbxHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cronExpressionEntity == null)
            {
                _cronExpressionEntity = new CronExpressionEntity();
                _cronExpressionEntity.CycleTypes = CycleTypes.M;

            }

            if (cbxHour.SelectedIndex > 0)
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
                _cronExpressionEntity.CycleTypes = CycleTypes.M;
            }

            if (cbxMinute.SelectedIndex > 0)
            {
                _cronExpressionEntity.Minute = cbxMinute.SelectedValue.ToString();
            }

            BindlstReecntTimes();
        }

        private void BindlstReecntTimes()
        {
            BindControl.BindListBox(lstRecentTimes, new List<KVEntity>());
            if (cbxHour.SelectedIndex == 0 || cbxMinute.SelectedIndex == 0 || chklstMonths.CheckedItems.Count == 0)
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
