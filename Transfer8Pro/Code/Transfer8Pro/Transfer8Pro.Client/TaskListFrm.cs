using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Core;
using Transfer8Pro.Core;
using Transfer8Pro.Core.Service;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Client
{
    public partial class TaskListFrm : BaseFrm
    {
        public TaskListFrm()
        {
            InitializeComponent();          
            pagerCtrl1.OnNextPageClicked += PagerCtrl1_OnNextPageClicked;
            pagerCtrl1.OnPreviousPageClicked += PagerCtrl1_OnPreviousPageClicked;
        }

        private void PagerCtrl1_OnPreviousPageClicked(object sender, MyEventArgs<int> e)
        {
            loadGridData(e.Value1 + 1);
        }

        private void PagerCtrl1_OnNextPageClicked(object sender, MyEventArgs<int> e)
        {
            loadGridData(e.Value1 + 1);
        }

        private void TaskListFrm_Load(object sender, EventArgs e)
        {
            BindDrowDownList();
            BindDataGridView();
        }

        private void BindDrowDownList()
        {
            BindControl.BindComboBox(cbxDataType, Common.GetDataTypeList());
            BindControl.BindComboBox(cbxCycleType, Common.GetCycleTypeList());
            BindControl.BindComboBox(cbxTaskStatus, Common.GetTaskStatusList());
            BindControl.BindComboBox(cbxEnabledStatus, Common.GetTaskEnabledStatusList());
        }

        private void BindDataGridView()
        {
            BindControl.InitDataGridView(dgvTaskList);
            dgvTaskList.CellClick += DgvTaskList_CellClick;
            dgvTaskList.CellMouseDown += DgvTaskList_CellMouseDown;
            loadGridData(1);
        }

        public void loadGridData(int pageIndex)
        {
            TaskEntity prms = BuildPrms();
            ParamtersForDBPageEntity<TaskEntity> pageData = new TaskService().GetTaskList(prms,pageIndex,pagerCtrl1.PageSize);
                       

            if (pageIndex == 1)
            {
                pagerCtrl1.TotalPages = pageData.Total;
            }
           

            fillGridRowData(pageData.DataList.ToList());
        }

        private TaskEntity BuildPrms()
        {
            TaskEntity taskEntity = new TaskEntity();
            if (!string.IsNullOrEmpty(txtTaskID.Text))
            {
                taskEntity.TaskID = txtTaskID.Text.Trim();
            }

            if (!string.IsNullOrEmpty(txtTaskName.Text))
            {
                taskEntity.TaskName = txtTaskName.Text.Trim();
            }

            if (cbxDataType.SelectedIndex > 0)
            {
                taskEntity.DataType = (DataTypes)cbxDataType.SelectedValue.ToString().ToInt();
            }

            if (cbxCycleType.SelectedIndex > 0)
            {
                taskEntity.CycleType = (CycleTypes)cbxCycleType.SelectedValue.ToString().ToInt();
            }

            if (cbxTaskStatus.SelectedIndex > 0)
            {
                taskEntity.TaskStatus = (TaskStatus)cbxTaskStatus.SelectedValue.ToString().ToInt();
            }

            if (cbxEnabledStatus.SelectedIndex > 0)
            {
                taskEntity.Enabled = cbxEnabledStatus.SelectedValue.ToString().ToInt();
            }
            return taskEntity;
        }

        private void DgvTaskList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                dgvTaskList.Rows[e.RowIndex].ContextMenuStrip = contextMenuStrip1;
                dgvTaskList.ClearSelection();
                dgvTaskList.Rows[e.RowIndex].Selected = true;
                dgvTaskList.CurrentCell = dgvTaskList.Rows[e.RowIndex].Cells[e.ColumnIndex];

                int enabled = dgvTaskList.Rows[e.RowIndex].Cells["Enabled"].Tag.ToString().ToInt();
                if (enabled == 1)
                {
                    停用ToolStripMenuItem.Text = "停用任务";
                }
                else
                {
                    停用ToolStripMenuItem.Text = "启用任务";
                }
            }
        }

        private void fillGridRowData(List<TaskEntity> list)
        {
            dgvTaskList.Rows.Clear();
            foreach (TaskEntity task in list)
            {
                int index = dgvTaskList.Rows.Add();
                DataGridViewRow row = dgvTaskList.Rows[index];

                row.Cells["TaskID"].Value = task.TaskID;
                row.Cells["TaskName"].Value = task.TaskName;
                row.Cells["Cron"].Value = task.Cron;

                row.Cells["DataType"].Tag = (int)task.DataType;
                row.Cells["DataType"].Value = task.DataType.ToString();

                row.Cells["CycleType"].Tag = (int)task.CycleType;
                row.Cells["CycleType"].Value = task.CycleType.ToString();

                row.Cells["TaskStatus"].Tag = (int)task.TaskStatus;
                row.Cells["TaskStatus"].Value = task.TaskStatus.ToString();

                if (task.RecentRunTime != DateTime.MinValue)
                {
                    row.Cells["RecentRunTime"].Value = task.RecentRunTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (task.NextFireTime != DateTime.MinValue)
                {
                    row.Cells["NextFireTime"].Value = task.NextFireTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                row.Cells["Enabled"].Tag = task.Enabled;
                row.Cells["Enabled"].Value = task.Enabled == 1 ? "启用" : "停用";
            }
        }

        private void DgvTaskList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {

            }
        }

        private void 修改任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                DataGridViewRow currrow = dgvTaskList.CurrentRow;
                string taskID = currrow.Cells["TaskID"].Value.ToString();
                AddTaskFrm frm = new AddTaskFrm(taskID,this);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void 复制任务IDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                string taskID = dgvTaskList.CurrentRow.Cells["TaskID"].Value.ToString();
                Clipboard.SetDataObject(taskID);
                base.ShowMessage("TaskID复制到剪贴板");
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void 查看执行计划时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                string taskName = dgvTaskList.CurrentRow.Cells["TaskName"].Value.ToString();
                string cron = dgvTaskList.CurrentRow.Cells["Cron"].Value.ToString();
                PreviewPalnExcuteTimeFrm frm = new PreviewPalnExcuteTimeFrm(taskName, cron);
                frm.ShowDialog();
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void 查看执行历史ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                string taskID = dgvTaskList.CurrentRow.Cells["TaskID"].Value.ToString();
                TaskHistoryListFrm frm = new TaskHistoryListFrm(taskID);
                frm.MdiParent = base.ParentForm;
                frm.Show();
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void 删除任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                if (MessageBox.Show("您确定要删除当前任务吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DataGridViewRow currrow = dgvTaskList.CurrentRow;
                    string taskID = currrow.Cells["TaskID"].Value.ToString();
                    if (new TaskService().Delete(taskID))
                    {
                        base.ShowMessage("任务删除成功");
                        this.loadGridData(1);
                    }
                    else
                    {
                        base.ShowErrorMessage("任务删除失败");
                        return;
                    }
                }
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void 查看任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                DataGridViewRow currrow = dgvTaskList.CurrentRow;
                string taskID = currrow.Cells["TaskID"].Value.ToString();
                AddTaskFrm frm = new AddTaskFrm(taskID,null,true);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadGridData(1);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
          
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtTaskID.Text = string.Empty;
            txtTaskName.Text = string.Empty;
            cbxDataType.SelectedIndex = 0;
            cbxCycleType.SelectedIndex = 0;
            cbxTaskStatus.SelectedIndex = 0;
            cbxEnabledStatus.SelectedIndex = 0;
            loadGridData(1);
        }

        private void 停用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTaskList.CurrentRow != null)
            {
                int enabled = dgvTaskList.CurrentRow.Cells["Enabled"].Tag.ToString().ToInt();
                string enabledTitle = enabled == 1 ? "停用" : "启用";
                if (MessageBox.Show($"您确定要{enabledTitle}当前任务吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {                  
                    string taskID = dgvTaskList.CurrentRow.Cells["TaskID"].Value.ToString();
                    if (new TaskService().UpdateTaskEnabledStatus(taskID, enabled == 1 ? 2 :1))
                    {
                        base.ShowMessage($"任务{enabledTitle}成功");
                        this.loadGridData(1);
                    }
                    else
                    {
                        base.ShowErrorMessage($"任务{enabledTitle}失败");
                        return;
                    }
                }
            }
            else
            {
                base.ShowMessage("未获取到当前行数据");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            AddTaskFrm frm = new AddTaskFrm(null, this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }
    }
}
