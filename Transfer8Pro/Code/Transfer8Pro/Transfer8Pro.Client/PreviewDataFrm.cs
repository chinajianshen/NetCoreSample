using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Client.Core;

namespace Transfer8Pro.Client
{
    public partial class PreviewDataFrm : Form
    {
        private DataTable _table;
        private DateTime _op;
        private DateTime _ed;

        public PreviewDataFrm(DataTable dt,DateTime op,DateTime ed)
        {
            InitializeComponent();
            _table = dt;
            _op = op;
            _ed = ed;
        }     

        private void PreviewDataFrm_Load(object sender, EventArgs e)
        {
            dgvList.AllowUserToAddRows = false;
            dgvList.AllowUserToDeleteRows = false;
            dgvList.ReadOnly = false;
            dgvList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvList.DataSource = _table;
            label1.Text = $"预览数据查询日期范围：{_op.ToString("yyyy-MM-dd HH:mm:ss")}<=日期<={_ed.ToString("yyyy-MM-dd HH:mm:ss")},共{_table.Rows.Count}条数据";
        }
    }

}
