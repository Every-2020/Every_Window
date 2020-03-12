using Every;
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
            CtrlSelectIdentity.SelectIdentityBackWardLoginPage += CtrlSelectIdentity_BackWardLoginPage; // 학생 or 직장인 & 대학생 선택
            CtrlStudentSignUp.StudentSignUpBackWardLoginPage += CtrlStudentSignUp_StudentSignUpBackWardLoginPage; // 학생 회원가입
            CtrlWorkerSignUp.WorkerSignUpBackWardLoginPage += CtrlWorkerSignUp_WorkerSignUpBackWardLoginPage; // 직장인 or 대학생 회원 가입

            CtrlStudentSignUp.LoadSearchSchoolWindow += CtrlStudentSignUp_LoadSearchSchoolWindow; // 학교검색 윈도우

            App.signUpData.signUpViewModel.OnStudentSignUpResultRecieved += SignUpViewModel_OnSignUpResultRecieved; // 학생회원가입 결과
            App.signUpData.signUpViewModel.OnWorkerSignUpResultReceived += SignUpViewModel_OnWorkerSignUpResultReceived;// 직장인회원가입 결과

            CtrlLogin.OnSignUpReceived += CtrlLogin_OnSignUpReceived; // 로그인 결과
        }

        #region 페이지 전환
        private void CtrlSelectIdentity_BackWardLoginPage(object sender, RoutedEventArgs e)
        {
            CtrlSelectIdentity.Visibility = Visibility.Collapsed;
            CtrlLogin.Visibility = Visibility.Visible;
        }

        private void CtrlStudentSignUp_StudentSignUpBackWardLoginPage(object sender, RoutedEventArgs e)
        {
            CtrlStudentSignUp.Visibility = Visibility.Collapsed;
            CtrlSelectIdentity.Visibility = Visibility.Visible;
        }

        private void CtrlWorkerSignUp_WorkerSignUpBackWardLoginPage(object sender, RoutedEventArgs e)
        {
            CtrlWorkerSignUp.Visibility = Visibility.Collapsed;
            CtrlSelectIdentity.Visibility = Visibility.Visible;
        }
        #endregion

        // 학교검색 윈도우 호출
        private void CtrlStudentSignUp_LoadSearchSchoolWindow(object sender, RoutedEventArgs e)
        {
            // 학교 검색 윈도우
            SearchSchoolWindow searchSchoolWindow = new SearchSchoolWindow();
            searchSchoolWindow.Show();
            //searchSchoolWindow.ShowDialog();
        }

        // 학생 회원가입
        private void SignUpViewModel_OnSignUpResultRecieved(TNetwork.Data.TResponse<TNetwork.Data.Nothing> signUpArgs)
        {
            if (signUpArgs.Status == (int)HttpStatusCode.Created)
            {
                CtrlStudentSignUp.Visibility = Visibility.Collapsed;
                MessageBox.Show("학생 회원가입에 성공하였습니다.");
                CtrlLogin.Visibility = Visibility.Visible;

                #region 학생 계정 생성 후 초기화
                App.searchSchoolWindow.tbInputSchool_Name.Text = string.Empty;
                App.signUpData.signUpViewModel.SchoolItems.Clear();
                CtrlStudentSignUp.tbInputEmail.Text = string.Empty;
                CtrlStudentSignUp.tbInputPw.Text = string.Empty;
                CtrlStudentSignUp.tbInputName.Text = string.Empty;
                CtrlStudentSignUp.tbInputPhone.Text = string.Empty;
                CtrlStudentSignUp.tbInputBirth_Year.Text = string.Empty;
                CtrlStudentSignUp.tbInputSchool_Id.Text = string.Empty;
                CtrlStudentSignUp.tbEmailDesc.Text = string.Empty;
                CtrlStudentSignUp.tbPwDesc.Text = string.Empty;
                CtrlStudentSignUp.tbPhoneNumDesc.Text = string.Empty;
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
                CtrlWorkerSignUp.cbSelectDuty.SelectedItem = null;
                CtrlWorkerSignUp.tbEmailDesc.Text = string.Empty;
                CtrlWorkerSignUp.tbPwDesc.Text = string.Empty;
                CtrlWorkerSignUp.tbPhoneNumDesc.Text = string.Empty;
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
                
                ctrlNavi.Visibility = Visibility.Visible;
                ctrlNavi.InitView();
            }
        }
    }
}
