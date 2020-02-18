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
            CtrlSelectIdentity.BackWardLoginPage += CtrlSelectIdentity_BackWardLoginPage;
            App.signUpData.signUpViewModel.OnStudentSignUpResultRecieved += SignUpViewModel_OnSignUpResultRecieved;
            App.signUpData.signUpViewModel.OnWorkerSignUpResultReceived += SignUpViewModel_OnWorkerSignUpResultReceived;
            CtrlLogin.OnSignUpReceived += CtrlLogin_OnSignUpReceived;
        }

        private void CtrlSelectIdentity_BackWardLoginPage(object sender, RoutedEventArgs e)
        {
            CtrlSelectIdentity.Visibility = Visibility.Collapsed;
            CtrlLogin.Visibility = Visibility.Visible;
        }

        // 학생 회원가입
        private void SignUpViewModel_OnSignUpResultRecieved(TNetwork.Data.TResponse<TNetwork.Data.Nothing> signUpArgs)
        {
            if(signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlStudentSignUp.Visibility = Visibility.Collapsed;
                MessageBox.Show("학생 회원가입에 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;

                #region 학생 계정 생성 후 초기화
                CtrlStudentSignUp.tbInputSchoolName.Text = string.Empty;
                App.signUpData.signUpViewModel.SchoolItems.Clear();
                CtrlStudentSignUp.tbInputEmail.Text = string.Empty;
                CtrlStudentSignUp.tbInputPw.Text = string.Empty;
                CtrlStudentSignUp.tbInputName.Text = string.Empty;
                CtrlStudentSignUp.tbInputPhone.Text = string.Empty;
                CtrlStudentSignUp.tbInputBirth_Year.Text = string.Empty;
                CtrlStudentSignUp.tbInputSchool_Id.Text = string.Empty;
                CtrlStudentSignUp.tbEmailDesc.Text = string.Empty;
                CtrlStudentSignUp.tbPwDesc.Text = string.Empty;
                #endregion
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

                #region 직장인 계정 생성 후 초기화 
                CtrlWorkerSignUp.tbInputEmail.Text = string.Empty;
                CtrlWorkerSignUp.tbInputPw.Text = string.Empty;
                CtrlWorkerSignUp.tbInputName.Text = string.Empty;
                CtrlWorkerSignUp.tbInputPhone.Text = string.Empty;
                CtrlWorkerSignUp.tbInputBirth_Year.Text = string.Empty;
                CtrlWorkerSignUp.tbInputWork_Place.Text = string.Empty;
                CtrlWorkerSignUp.tbInputWork_Category.Text = string.Empty;
                CtrlWorkerSignUp.tbEmailDesc.Text = string.Empty;
                CtrlWorkerSignUp.tbPwDesc.Text = string.Empty;
                #endregion
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
