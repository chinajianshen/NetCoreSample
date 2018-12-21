using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Core;
using Transfer8Pro.Core;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client.Controls
{
    public partial class TaskPlanDayCtrl : UserControl, ITaskPlanBase
    {
        CronExpressionEntity _cronExpressionEntity = null;
        public TaskPlanDayCtrl()
        {
            InitializeComponent();
        }

        private void TaskPlanDayCtrl_Load(object sender, EventArgs e)
        {
            BindData(_cronExpressionEntity);
        }       

        /// <summary>
        /// 获取或设置 Cron表达式
        /// </summary>
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

        /// <summary>
        /// 获取Cron表达式串
        /// </summary>
        /// <returns></returns>
        public string GetCronExpressionString()
        {
            return QuartzHelper.GenerateCronExpression(BuildCronExpression());
        }       

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public bool ValidateData()
        {
            if (rdoOneTimes.Checked)
            {
                if (cbxOneTimesHour.SelectedIndex == 0)
                {
                    MessageBox.Show("[执行一次单选按钮]右侧小时下列表不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxOneTimesHour.Focus();
                    return false;
                }
                if (cbxOneTimesMinute.SelectedIndex == 0)
                {
                    MessageBox.Show("[执行一次单选按钮]右侧分钟下列表不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxOneTimesMinute.Focus();
                    return false;
                }
            }
            else
            {
                if (cbxMultiTimesMinute.SelectedIndex == 0)
                {
                    MessageBox.Show("[执行多次单选按钮]右侧下列表不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxMultiTimesMinute.Focus();
                    return false;
                }
            }

            return true;
        }

        private void BindData(CronExpressionEntity cronExp)
        {
            //绑定执行一次 小时下拉列表
            BindControl.BindComboBox(cbxOneTimesHour, Common.DigitalHours());
            //绑定执行一次 分下拉列表
            BindControl.BindComboBox(cbxOneTimesMinute, Common.DigitalMinutes());
            //绑定执行多次 分下拉列表
            BindControl.BindComboBox(cbxMultiTimesMinute, Common.DigitalMultiTimesMinutes());

            if (cronExp != null)
            {
                if (cronExp.ExecutingOnce)
                {
                    cbxOneTimesHour.SelectedValue = cronExp.Hour;
                    cbxOneTimesMinute.SelectedValue = cronExp.Minute;
                    rdoOneTimes.Checked = true;
                }
                else
                {
                    if (cronExp.Minute != "0")
                    {
                        cbxMultiTimesMinute.SelectedValue = cronExp.Minute;
                    }

                    if (cronExp.Hour != "*" && cronExp.Hour != "0")
                    {
                        cbxMultiTimesMinute.SelectedValue = cronExp.Hour;
                    }
                   
                    rdoMultiTimes.Checked = true;
                }              
            }
            else
            {
                rdoOneTimes.Checked = true;
            }
        }

        private void rdoOneTimes_CheckedChanged(object sender, EventArgs e)
        {
            BindlstReecntTimes();
        }

        private void rdoMultiTimes_CheckedChanged(object sender, EventArgs e)
        {
            BindlstReecntTimes();
        }

        private void cbxOneTimesHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdoOneTimes.Checked = true;
            BindlstReecntTimes();
        }

        private void cbxOneTimesMinute_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdoOneTimes.Checked = true;
            BindlstReecntTimes();
        }

        private void cbxMultiTimesMinute_SelectedIndexChanged(object sender, EventArgs e)
        {           
            rdoMultiTimes.Checked = true;
            BindlstReecntTimes();
        }

        private CronExpressionEntity BuildCronExpression()
        {
            CronExpressionEntity cronExpression = new CronExpressionEntity();
            cronExpression.Second = "0";
            cronExpression.CycleTypes = CycleTypes.D;
            if (rdoOneTimes.Checked)
            {
                cronExpression.ExecutingOnce = true;
                cronExpression.Hour = cbxOneTimesHour.SelectedValue.ToString();
                cronExpression.Minute = cbxOneTimesMinute.SelectedValue.ToString();
            }
            else
            {
                cronExpression.ExecutingOnce = false;
                KVEntity kvEntity = (KVEntity)cbxMultiTimesMinute.SelectedItem;

                if (kvEntity.K.Contains("分"))
                {
                    cronExpression.Minute = kvEntity.V;
                    cronExpression.Hour = "*";
                }
                else
                {
                    cronExpression.Minute = "0";
                    cronExpression.Hour = kvEntity.V;
                }
            }
            return cronExpression;
        }

        private void BindlstReecntTimes()
        {
            BindControl.BindListBox(lstReecntTimes, new List<KVEntity>());           
          
            if (rdoOneTimes.Checked)
            {
                if (cbxOneTimesHour.SelectedIndex == 0 || cbxOneTimesMinute.SelectedIndex == 0)
                {
                    return;
                }
            }
            else
            {
                if (cbxMultiTimesMinute.SelectedIndex == 0)
                {
                    return;
                }
            }
            string cronString = QuartzHelper.GenerateCronExpression(BuildCronExpression());
            List<DateTime> list = QuartzHelper.GetNextFireTime(cronString, 8);
            List<KVEntity> kvList = list.Select(item => new KVEntity { K = item.ToString(), V = item.ToString() }).ToList();
            BindControl.BindListBox(lstReecntTimes, kvList);           
        }
    }
}
