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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangePassword_VM vm = new ChangePassword_VM();
            DataContext = vm;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword_VM vm = DataContext as ChangePassword_VM;
            if (vm != null)
            {
                if (vm.IsModelValid())
                {
                    Password_API_Put objectToPut = new Password_API_Put();
                    objectToPut.Password = WebApiClientHelper.MakeConfidentialMessage(vm.ConfirmPwd);
                    try
                    {
                        WebApiClientHelper.DoJsonRequest<Password_API_Put>(
                            GlobalData.GetResUri(string.Format("password/{0}", GlobalData.CurrentUserName)),
                            EnuHttpMethod.Put,
                            objToSend: objectToPut);
                    }
                    catch (ClientException ex)
                    {
                        vm.SetExtraError(ex.Message);
                        return;
                    }

                    WebApiClientHelper.Password = vm.ConfirmPwd;
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void buttonCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
