using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client.Controls
{
    public partial class PagerCtrl : UserControl
    {
        public PagerCtrl()
        {
            InitializeComponent();
        }

        public event EventHandler<MyEventArgs<int>> OnNextPageClicked;
        public event EventHandler<MyEventArgs<int>> OnPreviousPageClicked;

        private void RefreshPageNo()
        {
            label1.Text = string.Format(label1.Tag.ToString(), CurrentPageNo + 1, TotalPages == 0 ? 1 : TotalPages, _innerTotalCount);
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            CurrentPageNo--;
            if (CurrentPageNo == 0)
            {
                btnPrePage.Enabled = false;
            }
            if (CurrentPageNo < TotalPages - 1)
            {
                btnNextPage.Enabled = true;
            }
            if (this.OnPreviousPageClicked != null)
            {
                this.OnPreviousPageClicked(this, new MyEventArgs<int>() { Value1 = CurrentPageNo });
            }
            RefreshPageNo();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            CurrentPageNo++;
            if (CurrentPageNo > 0)
            {
                btnPrePage.Enabled = true;
            }
            if (CurrentPageNo >= TotalPages - 1)
            {
                btnNextPage.Enabled = false;
            }
            if (this.OnNextPageClicked != null)
            {
                this.OnNextPageClicked(this, new MyEventArgs<int>() { Value1 = CurrentPageNo });
            }
            RefreshPageNo();
        }

        public int CurrentPageNo { get; set; }

        private int _innerTotalCount;

        private int _totalpages;
        public int TotalPages
        {
            get { return _totalpages; }
            set
            {
                _innerTotalCount = value;
                _totalpages = value % PageSize > 0 ? value / PageSize + 1 : value / PageSize;
                CurrentPageNo = 0;
                RefreshPageNo();
                if (_totalpages <= 1)
                {
                    btnPrePage.Enabled = btnNextPage.Enabled = false;
                }
                else
                {
                    btnNextPage.Enabled = true;
                }
            }
        }

        private int _PageSize = 20;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

    }
}
