using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Controls;
using Transfer8Pro.Client.Core;
using Transfer8Pro.Core;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Client
{
    public partial class AddTaskFrm : BaseFrm
    {
        private string _TaskID;
        private bool _IsShow;
        private TaskEntity _TaskEntity = null;
        private TaskListFrm _ParentFrm = null;

        public AddTaskFrm()
        {
            InitializeComponent();
        }

        public AddTaskFrm(string taskID, TaskListFrm parentFrm,bool isShow = false) : this()
        {
            _TaskID = taskID;
            _IsShow = isShow;
            _ParentFrm = parentFrm;
        }

        private void AddTaskFrm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_TaskID))
            {
                _TaskEntity = new TaskService().Find(_TaskID);
                if (_TaskEntity == null)
                {
                    base.ShowErrorMessage("系统中未找到该任务");
                    return;
                }
            }

            BindData();

            if (_IsShow)
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.Gray;
            }
        }

        private void BindData()
        {
            //1绑定数据类型
            BindControl.BindComboBox(cbxDataTypes, Common.DataTypeList(), _TaskEntity != null ? ((int)_TaskEntity.DataType).ToString() : "");

            //2绑定数据库类型
            BindControl.BindComboBox(cbxDataBaseType, Common.DbTypeList(), _TaskEntity != null ? _TaskEntity.DataHandler : "");

            //3绑定执行计划           
            if (_TaskEntity == null)
            {
                //默认加载每天执行计划
                rdoDay.Checked = true;
            }
            else
            {
                txtTaskName.Text = _TaskEntity.TaskName;
                txtDbConn.Text = Common.DecryptData(_TaskEntity.DBConnectString_Hashed);
                txtSql.Text = _TaskEntity.SQL;

                if (_TaskEntity.CycleType == CycleTypes.D)
                {
                    rdoDay.Checked = true;
                }
                else if (_TaskEntity.CycleType == CycleTypes.W)
                {
                    rdoWeek.Checked = true;
                }
                else if (_TaskEntity.CycleType == CycleTypes.M)
                {
                    rdoMonth.Checked = true;
                }
                else
                {
                    rdoDay.Checked = true;
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSaveData())
            {
                #region 赋值数据
                if (_TaskEntity == null)
                {
                    _TaskEntity = new TaskEntity();
                    _TaskEntity.TaskID =  Guid.NewGuid().ToString("N");
                    _TaskEntity.Enabled = 1;
                    _TaskEntity.TaskStatus = TaskStatus.STOP;
                    _TaskEntity.IsDelete = false;
                }              
                _TaskEntity.DataType = (DataTypes)cbxDataTypes.SelectedValue.ToString().ToInt();
                _TaskEntity.CycleType = GetSelectedCycleType();
                _TaskEntity.Cron = GetTaskPlanCtrl().GetCronExpressionString();
                _TaskEntity.DataHandler = cbxDataBaseType.SelectedValue.ToString();
                _TaskEntity.DBConnectString_Hashed = Common.EncryptData(txtDbConn.Text.Trim());
                _TaskEntity.SQL = txtSql.Text.Trim();
                _TaskEntity.TaskName = txtTaskName.Text.Trim();               
              
                #endregion

                if (string.IsNullOrEmpty(_TaskID))
                {
                    #region 保存
                    int status = new TaskService().Insert(_TaskEntity);
                    if (status == 1)
                    {
                        base.ShowMessage("数据保存成功");
                        this.Close();
                        if (_ParentFrm != null)
                        {
                            _ParentFrm.loadGridData(1);
                        }
                    }
                    else if (status == 2)
                    {
                        base.ShowMessage("系统中存在相同的任务名称");
                    }
                    else
                    {
                        base.ShowErrorMessage("数据保存失败");
                    }
                    #endregion
                }
                else
                {
                    #region 修改
                    _TaskEntity.ModifyTime = DateTime.Now;
                    bool isSuccess = new TaskService().Update(_TaskEntity);
                    if (isSuccess)
                    {
                        base.ShowMessage("数据保存成功");
                        this.Close();
                        if (_ParentFrm != null)
                        {
                            _ParentFrm.loadGridData(1);
                        }
                    }
                    else
                    {
                        base.ShowErrorMessage("数据保存失败");
                    }
                    #endregion
                }           
            }
        }

        private CycleTypes GetSelectedCycleType()
        {
            CycleTypes cycleType = CycleTypes.D;
            if (rdoDay.Checked)
            {
                cycleType = CycleTypes.D;
            }
            else if (rdoMonth.Checked)
            {
                cycleType = CycleTypes.M;
            }
            else if (rdoWeek.Checked)
            {
                cycleType = CycleTypes.W;
            }
           
            return cycleType;
        }

        private bool ValidateSaveData()
        {
            if (string.IsNullOrEmpty(txtTaskName.Text))
            {
                base.ShowMessage("任务名称为必填项");
                txtTaskName.Focus();
                return false;
            }

            if (cbxDataTypes.SelectedIndex == 0)
            {
                base.ShowMessage("请选择数据类型");
                cbxDataTypes.Focus();
                return false;
            }

            if (cbxDataBaseType.SelectedIndex == 0)
            {
                base.ShowMessage("请选择数据库类型");
                cbxDataBaseType.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtDbConn.Text))
            {
                base.ShowMessage("数据库连接字符串配置为必填项");
                txtDbConn.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtSql.Text))
            {
                base.ShowMessage("SQL语句配置为必填项");
                txtSql.Focus();
                return false;
            }

            try
            {
                Common.DBConnectOpen(txtDbConn.Text.Trim(), cbxDataBaseType.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"连接数据库失败，请检查数据库连接字符串！异常信息[{ex.Message}]");
                return false;
            }

            try
            {
                //查询近1天数据
                DateTime op = DateTime.Now.AddDays(-1).Date;
                DateTime ed = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
                DataTable table = Common.GetDataList(txtDbConn.Text.Trim(), cbxDataBaseType.SelectedValue.ToString(), txtSql.Text.Trim(), op, ed);
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"连接数据库成功，但执行SQL语句出现错误，错误信息[{ex.Message}]");
                return false;
            }

            if (!rdoDay.Checked && !rdoWeek.Checked && !rdoMonth.Checked)
            {
                base.ShowErrorMessage("请选择执行计划");
                return false;
            }

            ITaskPlanBase taskPlanBase = GetTaskPlanCtrl();
            if (taskPlanBase == null)
            {
                base.ShowErrorMessage("执行计划部分控件丢失");
                return false;
            }

            if (!taskPlanBase.ValidateData())
            {
                return false;
            }

            return true;
        }

        private void cbxDataBaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            KVEntity<int> entity = cbxDataBaseType.SelectedItem as KVEntity<int>;
            if (entity.V != "-999")
            {
                KVEntity dbReservedEntity = Common.DbReservedString((DbTypes)entity.T1);
                if (dbReservedEntity != null)
                {
                    txtDbConn.Text = dbReservedEntity.K;
                    txtSql.Text = dbReservedEntity.V;
                }
                else
                {
                    txtDbConn.Text = "";
                    txtSql.Text = "";
                    ;
                }
            }
            else
            {
                txtDbConn.Text = "";
                txtSql.Text = "";
            }
        }

        private ITaskPlanBase GetTaskPlanCtrl()
        {
            Control[] ctrls = pnlPlan.Controls.Find("TaskPlanCtrl", false);
            if (ctrls.Length > 0)
            {
                return ctrls[0] as ITaskPlanBase;
            }
            return null;
        }

        private void btnDbCheck_Click(object sender, EventArgs e)
        {
            if (cbxDataBaseType.SelectedIndex == 0)
            {
                base.ShowMessage("请选择数据库类型");
                cbxDataBaseType.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtDbConn.Text))
            {
                base.ShowMessage("请配置数据库连接字符串");
                txtDbConn.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtSql.Text))
            {
                base.ShowMessage("请配置SQL语句");
                txtSql.Focus();
                return;
            }
            try
            {
                Common.DBConnectOpen(txtDbConn.Text.Trim(), cbxDataBaseType.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"连接数据库失败，请检查数据库连接字符串！异常信息[{ex.Message}]");
                return;
            }

            try
            {
                //查询近半月数据
                DateTime op = DateTime.Now.AddDays(-15).Date;
                DateTime ed = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
                DataTable table = Common.GetDataList(txtDbConn.Text.Trim(), cbxDataBaseType.SelectedValue.ToString(), txtSql.Text.Trim(), op, ed);
                PreviewDataFrm frm = new PreviewDataFrm(table, op, ed);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                base.ShowErrorMessage($"连接数据库成功，但执行SQL语句出现错误，错误信息[{ex.Message}]");
                return;
            }
        }

        private void rdoDay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDay.Checked)
            {
                TaskPlanDayCtrl ctrl = new TaskPlanDayCtrl();
                ctrl.Name = "TaskPlanCtrl";
                if (_TaskEntity != null && _TaskEntity.CycleType == CycleTypes.D)
                {
                    CronExpressionEntity cronExp = QuartzHelper.ResolveCronExpression(_TaskEntity.Cron);
                    ctrl.CronExpressionEntity = cronExp;
                }
                pnlPlan.Controls.Clear();
                pnlPlan.Controls.Add(ctrl);
            }

        }

        private void rdoWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoWeek.Checked)
            {
                TaskPlanWeekCtrl ctrl = new TaskPlanWeekCtrl();
                ctrl.Name = "TaskPlanCtrl";
                if (_TaskEntity != null && _TaskEntity.CycleType == CycleTypes.W)
                {
                    CronExpressionEntity cronExp = QuartzHelper.ResolveCronExpression(_TaskEntity.Cron);
                    ctrl.CronExpressionEntity = cronExp;
                }
                pnlPlan.Controls.Clear();
                pnlPlan.Controls.Add(ctrl);
            }
        }

        private void rdoMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonth.Checked)
            {
                TaskPlanMonthCtrl ctrl = new TaskPlanMonthCtrl();
                ctrl.Name = "TaskPlanCtrl";
                if (_TaskEntity != null && _TaskEntity.CycleType == CycleTypes.M)
                {
                    CronExpressionEntity cronExp = QuartzHelper.ResolveCronExpression(_TaskEntity.Cron);
                    ctrl.CronExpressionEntity = cronExp;
                }
                pnlPlan.Controls.Clear();
                pnlPlan.Controls.Add(ctrl);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
