using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.Models;
using CommLib;
using WebApiKit;
using WebApiContract.Models;

namespace Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUserName.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string pwd = Md5.MD5TwiceEncode("123456");
            Login_VM vmLogin = DataContext as Login_VM;
            if (vmLogin == null)
                return;

            if (vmLogin.IsModelValid())
            {
                WebApiClientHelper.UserName = vmLogin.UserName;
                WebApiClientHelper.Password = vmLogin.Password;
                UserInfo_API_Get userInfo;
                try
                {
                    userInfo = WebApiClientHelper.DoJsonRequest<UserInfo_API_Get>(GlobalData.GetResUri("entrance"), EnuHttpMethod.Get);
                }
                catch (ClientException ex)
                {
                    vmLogin.SetExtraError(ex.Message);
                    return;
                }

                //验证成功，记录用户信息
                GlobalData.CurrentUserName = userInfo.UserName;
                GlobalData.CurrentDispName = userInfo.RealName;
                GlobalData.CurrentRole = userInfo.Role;

                //转入主窗口
                MainWindow main = new MainWindow();
                Application.Current.MainWindow = main;
                Close();
                main.Show();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
