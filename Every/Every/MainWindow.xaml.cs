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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.signUpData.signUpViewModel.OnStudentSignUpResultRecieved += SignUpViewModel_OnSignUpResultRecieved;
            App.signUpData.signUpViewModel.OnWorkerSignUpResultReceived += SignUpViewModel_OnWorkerSignUpResultReceived;
            CtrlLogin.OnSignUpReceived += CtrlLogin_OnSignUpReceived;
        }        

        // 학생 회원가입
        private void SignUpViewModel_OnSignUpResultRecieved(TNetwork.Data.TResponse<TNetwork.Data.Nothing> signUpArgs)
        {
            if(signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlStudentSignUp.Visibility = Visibility.Collapsed;
                MessageBox.Show("학생 회원가입에 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;
            }
        }

        // 직장인 회원가입
        private void SignUpViewModel_OnWorkerSignUpResultReceived(TNetwork.Data.TResponse<TNetwork.Data.Nothing> signUpArgs)
        {
            if (signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlWorkerSignUp.Visibility = Visibility.Collapsed;
                MessageBox.Show("직장인 회원가입에 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;
            }
        }

        private void CtrlLogin_OnSignUpReceived(object sender, RoutedEventArgs e)
        {
            CtrlLogin.Visibility = Visibility.Collapsed;
            CtrlSelectIdentity.Visibility = Visibility.Visible;

            // 학생 or 직장인 선택 후 각각의 해당 컨트롤 호출
            CtrlSelectIdentity.OnCreateStudentAccount += SelectIdentity_OnCreateStudentAccount;
            CtrlSelectIdentity.OnCreateWorkerAccount += SelectIdentity_OnCreateWorkerAccount;
        }

        #region 회원가입 선택
        private void SelectIdentity_OnCreateStudentAccount(object Sender, RoutedEventArgs e)
        {
            CtrlSelectIdentity.Visibility = Visibility.Collapsed;
            CtrlStudentSignUp.Visibility = Visibility.Visible;
        }

        private void SelectIdentity_OnCreateWorkerAccount(object Sender, RoutedEventArgs e)
        {
            CtrlSelectIdentity.Visibility = Visibility.Collapsed;
            CtrlWorkerSignUp.Visibility = Visibility.Visible;
        }
        #endregion
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
