using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Every_AdminWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.signupData.signUpViewModel.OnSignUpResultRecieved += SignUpViewModel_OnSignUpResultRecieved1;
            CtrlLogin.OnSignUpReceived += CtrlLogin_OnSignUpReceived;
        }

        private void SignUpViewModel_OnSignUpResultRecieved1(TNetwork.Data.TResponse<TNetwork.Data.Nothing> signUpArgs)
        {
            if(signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlSignUp.Visibility = Visibility.Collapsed;
                MessageBox.Show("회원가입에 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;
            }
        }

        private void CtrlLogin_OnSignUpReceived(object sender, bool success)
        {
            CtrlLogin.Visibility = Visibility.Collapsed;
            CtrlSignUp.Visibility = Visibility.Visible;
        }

        private async void LoginCtrl_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                CtrlLogin.Visibility = Visibility.Collapsed;
                MessageBox.Show("로그인에 성공하셨습니다!");
            }
        }
    }
}
