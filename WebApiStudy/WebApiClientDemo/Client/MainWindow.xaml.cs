using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommLib;
using Microsoft.Win32;
using WebApiKit;
using WebApiContract.Models;
using AutoMapper;
using Client.Models;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CMD_GoToEditMode(object sender, ExecutedRoutedEventArgs e)
        {
            UserInfo_VM ui_vm = gridMyInfoTab.DataContext as UserInfo_VM;
            if (ui_vm != null)
            {
                ui_vm.ModelType = EnuModelType.Edit;
                ui_vm.TakeSnapshot();
            }
        }

        private void CMD_GoToViewMode(object sender, ExecutedRoutedEventArgs e)
        {
            UserInfo_VM ui_vm = gridMyInfoTab.DataContext as UserInfo_VM;
            if (ui_vm != null)
            {
                ui_vm.RecoverFromSnapshot();
                ui_vm.ModelType = EnuModelType.View;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbLogingInfo.Text = "用户信息：" + GlobalData.CurrentDispName + " - " + GlobalData.CurrentRole;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            LoadMyInfo();
            RetrieveMyAvatar();
        }

        private bool LoadMyInfo(bool bShowSuccessfulPopup = false)
        {
            //加载数据
            UserInfo_API_Get uiToGet;
            try
            {
                uiToGet = WebApiClientHelper.DoJsonRequest<UserInfo_API_Get>(GlobalData.GetResUri(string.Format("usersinfo/{0}", GlobalData.CurrentUserName)), EnuHttpMethod.Get);

            }
            catch (ClientException ex)
            {
                Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = ex.Message, AlertType = EnuPopupAlertType.Error }, this);
                return false;
            }

            UserInfo_VM ui_vm = Mapper.Map<UserInfo_VM>(uiToGet);
            ui_vm.ModelType = EnuModelType.View;
            gridMyInfoTab.DataContext = ui_vm;

            if (bShowSuccessfulPopup)
            {
                Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = "刷新我的信息成功.", AlertType = EnuPopupAlertType.Info }, this);
            }

            return true;
        }

        private bool LoadAllUsersInfo(bool bShowSuccessfulPopup = false)
        {
            //加载数据
            IEnumerable<UserInfo_API_Get> uisToGet;
            try
            {
                uisToGet =
                    WebApiClientHelper.DoJsonRequest<IEnumerable<UserInfo_API_Get>>(GlobalData.GetResUri("usersinfo"),
                                                                                    EnuHttpMethod.Get);
            }
            catch (ClientException ex)
            {
                Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = ex.Message, AlertType = EnuPopupAlertType.Error }, this);
                return false;
            }

            IEnumerable<UserInfo_VM> uis_vm = Mapper.Map<IEnumerable<UserInfo_VM>>(uisToGet);
            foreach (var ui in uis_vm)
            {
                ui.ModelType = EnuModelType.View;
            }
            gridAllUsersInfoTab.DataContext = uis_vm;

            if (bShowSuccessfulPopup)
            {
                Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = "刷新所有用户信息成功.", AlertType = EnuPopupAlertType.Info }, this);
            }

            return true;
        }

        private delegate void ShowPopupAlertMethod(ShowPopupAlertParam showParam);

        private void ShowPopupAlert(ShowPopupAlertParam showParam)
        {
            if (showParam != null)
            {
                textblockPopupAlert.Text = showParam.AlertMessage;
                switch (showParam.AlertType)
                {
                    case EnuPopupAlertType.Info:
                        imgPopupAlertIcon.Source = new BitmapImage(new Uri(@"Content\info_icon.png", UriKind.Relative));
                        borderPopupAlert.BorderBrush = new SolidColorBrush(Color.FromRgb(64, 128, 255));
                        break;
                    case EnuPopupAlertType.Alert:
                        imgPopupAlertIcon.Source = new BitmapImage(new Uri(@"Content\alert_icon.png", UriKind.Relative));
                        borderPopupAlert.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        break;
                    case EnuPopupAlertType.Error:
                        imgPopupAlertIcon.Source = new BitmapImage(new Uri(@"Content\error_icon.png", UriKind.Relative));
                        borderPopupAlert.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;
                }
            }
            else
            {
                textblockPopupAlert.Text = "";
            }
            popupAlert.IsOpen = true;
        }

        private void CMD_ShowPopupAlert(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new ShowPopupAlertMethod(ShowPopupAlert), e.Parameter as ShowPopupAlertParam);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadMyInfo(true);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            UserInfo_VM ui_vm = gridMyInfoTab.DataContext as UserInfo_VM;

            if (ui_vm != null)
            {
                if (ui_vm.IsModelValid())
                {
                    try
                    {
                        UserInfo_API_Put userput = Mapper.Map<UserInfo_VM, UserInfo_API_Put>(ui_vm);
                        WebApiClientHelper.DoJsonRequest<UserInfo_API_Put>(GlobalData.GetResUri(string.Format("usersinfo/{0}", ui_vm.UserName)), EnuHttpMethod.Put, objToSend: userput, tick: ui_vm.UpdateTicks);
                    }
                    catch (ClientException ex)
                    {
                        ui_vm.SetExtraError(ex.Message); //这是Popup以外的另一种异常显示方式
                        return;
                    }

                    LoadMyInfo();

                    Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = "修改信息成功.", AlertType = EnuPopupAlertType.Info }, this);

                    Commands.GoToViewMode.Execute(null, this);
                }
            }
        }

        private void btnGetAll_Click(object sender, RoutedEventArgs e)
        {
            LoadAllUsersInfo(true);
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword pw = new ChangePassword { Owner = this, ShowInTaskbar = false };
            pw.ShowDialog();
            if (pw.DialogResult == true)
            {
                Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = "密码修改成功", AlertType = EnuPopupAlertType.Info }, this);
            }
        }

        private void RetrieveMyAvatar()
        {
            Stream stm;
            try
            {
                stm = WebApiClientHelper.DoStreamRequest(GlobalData.GetResUri(string.Format("avatars/{0}", GlobalData.CurrentUserName)), EnuHttpMethod.Get);
            }
            catch (ClientException) //忽略获取失败的错误
            {
                return;
            }

            if (stm == null)
            {
                imgAvatar.Source = null;
                return;
            }

            SetAvatarImage(stm);
        }

        private void btnClearAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.OK == MessageBox.Show("要删除头像吗？", "确认", MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
                try
                {
                    WebApiClientHelper.DoJsonRequest<int>(GlobalData.GetResUri(string.Format("avatars/{0}", GlobalData.CurrentUserName)),
                                                          EnuHttpMethod.Delete);
                }
                catch (ClientException ex)
                {
                    Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = ex.Message, AlertType = EnuPopupAlertType.Error }, this);
                    return;
                }
                SetAvatarImage(null);
            }
        }

        private void btnUploadAvatar_Click(object sender, RoutedEventArgs e)
        {
            string avatar = _OpenFileDialog("选择头像", "jpg文件(*.jpg)|*.jpg");
            if (!string.IsNullOrEmpty(avatar))
            {
                using(FileStream fs = File.Open(avatar, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        WebApiClientHelper.DoStreamRequest(
                            GlobalData.GetResUri(string.Format("avatars/{0}", GlobalData.CurrentUserName)),
                            EnuHttpMethod.Put, fs);
                    }
                    catch (ClientException ex)
                    {
                        Commands.ShowPopupAlert.Execute(new ShowPopupAlertParam { AlertMessage = ex.Message, AlertType = EnuPopupAlertType.Error }, this);
                        return;
                    }
                }

                //设置头像(需要重新打开文件流并将它转为MemoryStream)
                using (FileStream fs = File.Open(avatar, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (fs.Length > 0)
                    {
                        byte[] memory = new byte[fs.Length];
                        fs.Read(memory, 0, (int)fs.Length);
                        SetAvatarImage(new MemoryStream(memory));
                    }
                }
            }
        }

        private static string _OpenFileDialog(string strTitle, string strFileType)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (!string.IsNullOrEmpty(strFileType))
                op.Filter = strFileType;
            if (!string.IsNullOrEmpty(strTitle))
                op.Title = strTitle;
            op.ShowDialog();
            return op.FileName;
        }


        private void SetAvatarImage(Stream stm)
        {
            if (stm==null)
            {
                imgAvatar.Source = null;
            }
            else
            {
                BitmapImage bitmap = new BitmapImage();
                
                bitmap.BeginInit();
                bitmap.StreamSource = stm;
                bitmap.EndInit();
                imgAvatar.Source = bitmap;
            }
        }
    }
}
