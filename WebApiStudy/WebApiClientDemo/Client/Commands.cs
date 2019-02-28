using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Client
{
    public enum EnuPopupAlertType
    {
        Info,
        Alert,
        Error
    }

    public class ShowPopupAlertParam
    {
        /// <summary>
        /// 用于显示不同的图标
        /// </summary>
        public EnuPopupAlertType AlertType { get; set; }

        /// <summary>
        /// 警示Popup的显示文本
        /// </summary>
        public string AlertMessage { get; set; }
    }

    static class Commands
    {
        private static readonly RoutedUICommand m_cmdGoToEditMode = new RoutedUICommand("GoToEditMode", "GoToEditMode", typeof(Commands), null);
        private static readonly RoutedUICommand m_cmdGotoViewMode = new RoutedUICommand("GoToViewMode", "GoToViewMode", typeof(Commands), null);
        private static readonly RoutedUICommand m_cmdShowPopupAlert = new RoutedUICommand("ShowPopupAlert", "ShowPopupAlert", typeof(Commands), null);

        /// <summary>
        /// 切换至编辑模式
        /// </summary>
        public static RoutedUICommand GoToEditMode
        {
            get { return m_cmdGoToEditMode; }
        }

        /// <summary>
        /// 切换至查看模式（重新从DB获取数据）
        /// </summary>
        public static RoutedUICommand GoToViewMode
        {
            get { return m_cmdGotoViewMode; }
        }

        /// <summary>
        /// 要求主界面出现一个信息/警示/错误
        /// </summary>
        public static RoutedUICommand ShowPopupAlert
        {
            get { return m_cmdShowPopupAlert; }
        }
    }
}
