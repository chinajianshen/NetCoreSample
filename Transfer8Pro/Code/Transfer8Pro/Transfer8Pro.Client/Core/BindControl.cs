using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Transfer8Pro.Entity;

namespace Transfer8Pro.Client.Core
{
    public class BindControl
    {

        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindComboBox(ComboBox cbx, List<KVEntity> kVEntities, int selectedIndex = 0, string displayMember = "K", string valueMember = "V")
        {
            if (cbx == null)
            {
                return;
            }

            cbx.DisplayMember = displayMember;
            cbx.ValueMember = valueMember;
            cbx.DataSource = kVEntities;
            cbx.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindComboBox(ComboBox cbx, List<KVEntity> kVEntities, string selectedValue, string displayMember = "K", string valueMember = "V")
        {
            if (cbx == null)
            {
                return;
            }

            cbx.DisplayMember = displayMember;
            cbx.ValueMember = valueMember;
            cbx.DataSource = kVEntities;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                cbx.SelectedValue = selectedValue;
            }           
        }

        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindComboBox<T>(ComboBox cbx, List<KVEntity<T>> kVEntities, int selectedIndex = 0, string displayMember = "K", string valueMember = "V")
        {
            if (cbx == null)
            {
                return;
            }

            cbx.DisplayMember = displayMember;
            cbx.ValueMember = valueMember;
            cbx.DataSource = kVEntities;
            cbx.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindComboBox<T>(ComboBox cbx, List<KVEntity<T>> kVEntities, string selectedValue, string displayMember = "K", string valueMember = "V")
        {
            if (cbx == null)
            {
                return;
            }

            cbx.DisplayMember = displayMember;
            cbx.ValueMember = valueMember;
            cbx.DataSource = kVEntities;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                cbx.SelectedValue = selectedValue;
            }           
        }

        /// <summary>
        /// 绑定ListBox控件
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        public static void BindListBox(ListBox lst, List<KVEntity> kVEntities, string displayMember = "K", string valueMember = "V")
        {
            lst.DisplayMember = displayMember;
            lst.ValueMember = valueMember;
            lst.DataSource = kVEntities;
        }

        /// <summary>
        /// 绑定ListBox控件
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        public static void BindListBox<T>(ListBox lst, List<KVEntity<T>> kVEntities, string displayMember = "K", string valueMember = "V")
        {
            lst.DisplayMember = displayMember;
            lst.ValueMember = valueMember;
            lst.DataSource = kVEntities;
        }


        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindCheckedListBox(CheckedListBox chklst, List<KVEntity> kVEntities, int selectedIndex = -1, string displayMember = "K", string valueMember = "V")
        {
            if (chklst == null)
            {
                return;
            }

            chklst.DataSource = kVEntities;
            chklst.DisplayMember = displayMember;
            chklst.ValueMember = valueMember;
           
            if (selectedIndex > -1)
            {
                chklst.SelectedIndex = selectedIndex;
            }
           
        }

        /// <summary>
        /// 绑定列表控件
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="kVEntities"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="selectedIndex">列表默认选中索引项</param>
        public static void BindCheckedListBox(CheckedListBox chklst, List<KVEntity> kVEntities, string selectedValue, string displayMember = "K", string valueMember = "V")
        {
            if (chklst == null)
            {
                return;
            }

            chklst.DataSource = kVEntities;
            chklst.DisplayMember = displayMember;
            chklst.ValueMember = valueMember;
         

            if (!string.IsNullOrEmpty(selectedValue))
            {
                for (int i = 0; i < chklst.Items.Count; i++)
                {
                    KVEntity kv = chklst.Items[i] as KVEntity;

                    foreach (string item in selectedValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                         if (kv.V == item)
                        {
                            chklst.SetItemChecked(i, true);
                        }
                    }
                }                        
            }
        }

        /// <summary>
        ///  初始化表格控件
        /// </summary>
        /// <param name="dgv"></param>
        public static void InitDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.AllowUserToAddRows = false; //禁止自带添加行
            dgv.AllowUserToDeleteRows = false; //禁止自带删除行
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            for (int index = 0; index < dgv.Columns.Count; index++)
            {
                dgv.Columns[index].ReadOnly = true;
            }
        }
    }
}
