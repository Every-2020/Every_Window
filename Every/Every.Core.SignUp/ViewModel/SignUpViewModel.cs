using Every.Core.SignUp.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TNetwork.Data;

namespace Every.Core.SignUp.ViewModel
{
    public class SignUpViewModel : BindableBase
    {
        SignUpService signUpService = new SignUpService();

        public delegate void OnStudentSignUpResultReceivedHandler(TResponse<Nothing> signUpArgs);
        public event OnStudentSignUpResultReceivedHandler OnStudentSignUpResultRecieved;

        public delegate void OnWorkerSignUpResultReceivedHandler(TResponse<Nothing> signUpArgs);
        public event OnWorkerSignUpResultReceivedHandler OnWorkerSignUpResultReceived;

        private bool Check_Result; // 정규식 검증 결과 여부 확인
        private bool Check_Email_OverLap = false; // 이메일 중복 확인 버튼 클릭 여부 확인

        #region Properties
        #region 학생 & 직장인 공통 Properties
        private string _inputEmail;
        public string InputEmail
        {
            get => _inputEmail;
            set
            {
                SetProperty(ref _inputEmail, value);
            }
        }

        private string _inputPw;
        public string InputPw
        {
            get => _inputPw;
            set
            {
                SetProperty(ref _inputPw, value);

                if (value.Length >= 8 && value.Length <= 20)
                {
                    Pw_Desc = "사용가능한 비밀번호 입니다.";
                    Pw_Desc_Foreground = Brushes.LightGreen;
                }
                else
                {
                    Pw_Desc = "비밀번호 자릿수를 다시 확인해 주세요.";
                    Pw_Desc_Foreground = Brushes.Red;
                }
            }
        }

        private string _inputName;
        public string InputName
        {
            get => _inputName;
            set
            {
                SetProperty(ref _inputName, value);
            }
        }

        private string _inputPhone;
        public string InputPhone
        {
            get => _inputPhone;
            set
            {
                SetProperty(ref _inputPhone, value);
            }
        }

        private int? _inputBirth_Year;
        public int? InputBirth_Year
        {
            get => _inputBirth_Year;
            set
            {
                SetProperty(ref _inputBirth_Year, value);
            }
        }

        private string _eamil_Desc;
        public string Email_Desc
        {
            get => _eamil_Desc;
            set
            {
                SetProperty(ref _eamil_Desc, value);
            }
        }

        private System.Windows.Media.Brush _email_Desc_Foreground;
        public System.Windows.Media.Brush Email_Desc_Foreground
        {
            get => _email_Desc_Foreground;
            set
            {
                SetProperty(ref _email_Desc_Foreground, value);
            }
        }

        private string _pw_Desc;
        public string Pw_Desc
        {
            get => _pw_Desc;
            set
            {
                SetProperty(ref _pw_Desc, value);
            }
        }

        private System.Windows.Media.Brush _pw_Desc_Foreground;
        public System.Windows.Media.Brush Pw_Desc_Foreground
        {
            get => _pw_Desc_Foreground;
            set
            {
                SetProperty(ref _pw_Desc_Foreground, value);
            }
        }

        #endregion

        #region 학생 전용 Properties
        private string _inputSchool_Id;
        public string InputSchool_Id
        {
            get => _inputSchool_Id;
            set
            {
                SetProperty(ref _inputSchool_Id, value);
           }
        }

        // Query Parameters, 학교 검색시에 사용
        private string _inputschool_Name;
        public string InputSchool_Name
        {
            get => _inputschool_Name;
            set
            {
                SetProperty(ref _inputschool_Name, value);
            }
        }

        // 학교 정보 저장
        private ObservableCollection<Model.School> _schoolItems = new ObservableCollection<Model.School>();
        public ObservableCollection<Model.School> SchoolItems
        {
            get => _schoolItems;
            set
            {
                SetProperty(ref _schoolItems, value);
            }
        }

        #endregion

        #region 직장인 전용 Properties
        private string _inputwork_Place;
        public string InputWork_Place
        {
            get => _inputwork_Place;
            set
            {
                SetProperty(ref _inputwork_Place, value);
            }
        }

        private int? _inputwork_Category;
        public int? InputWork_Category
        {
            get => _inputwork_Category;
            set
            {
                SetProperty(ref _inputwork_Category, value);
            }
        }
        #endregion

        // SettingHttpRequest
        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value.Trim());
        }

        #region Commands
        public ICommand StudentSignUpCommand { get; set; }
        public ICommand WorkerSignUpCommand { get; set; }
        public ICommand SearchSchoolCommand { get; set; }
        public ICommand CheckEmailOverLapCommand { get; set; }
        #endregion
        #endregion

        // 생성자
        public SignUpViewModel()
        {
            StudentSignUpCommand = new DelegateCommand(OnStudentSignUp, CanSignUp)/*.ObservesProperty(() => InputEmail)
                                                                                  .ObservesProperty(() => InputPw)
                                                                                  .ObservesProperty(() => InputName)
                                                                                  .ObservesProperty(() => InputPhone)
                                                                                  .ObservesProperty(() => InputBirth_Year)*/
                                                                                  .ObservesProperty(() => InputSchool_Id);
            WorkerSignUpCommand = new DelegateCommand(OnWorkerSignUp, CanSignUp)/*.ObservesProperty(() => InputEmail)
                                                                                .ObservesProperty(() => InputPw)
                                                                                .ObservesProperty(() => InputName)
                                                                                .ObservesProperty(() => InputPhone)
                                                                                .ObservesProperty(() => InputBirth_Year)
                                                                                .ObservesProperty(() => InputWork_Place)*/
                                                                                .ObservesProperty(() => InputWork_Category);
            SearchSchoolCommand = new DelegateCommand(OnSearchSchool, CanSearchSchool).ObservesProperty(() => InputSchool_Name);
            CheckEmailOverLapCommand = new DelegateCommand(OnCheckEmailOverLap, CanCheckEmailOverLap).ObservesProperty(() => InputEmail);
        }

        #region 회원 가입 Command
        private bool CanSignUp()
        {
            return (InputSchool_Id != null) && (InputSchool_Id != "") && (InputSchool_Id != string.Empty);
        }

        private void OnStudentSignUp()
        {
            if(Check_Email_OverLap == true)
            { 
                CheckRegularExpression(0);
            }
            else
            {
                Email_Desc = "이메일 중복 확인을 해주세요.";
                Email_Desc_Foreground = Brushes.Black;
            }
        }

        private void OnWorkerSignUp()
        {
            if(Check_Email_OverLap == true)
            { 
                CheckRegularExpression(1);
            }
        }

        private void CheckRegularExpression(int Distinguish_Identity)
        {
            // 휴대전화 입력시 하이픈("-")을 입력한 경우
            if (InputPhone.Contains("-"))
            {
                // 하이픈이 있는 휴대전화 번호의 정규식 확인
                HypenPhoneNumRegularExpressionCheck(InputPhone);

                if (Check_Result == true && Distinguish_Identity == 0)
                {
                    StudentSignUp();
                }
                else if(Check_Result == true && Distinguish_Identity == 1)
                {
                    WorkerSignUp();
                }
            }
            else // 휴대전화 입력시 하이픈("-")을 입력하지 않은 경우
            {
                // 하이픈이 없는 휴대전화 번호의 정규식 확인
                PhoneNumRegularExpressionCheck(InputPhone);

                // 정규식 확인 후 자동으로 하이픈 입력
                AutoInput_Hyphen(InputPhone);

                if (Check_Result == true && Distinguish_Identity == 0)
                {
                    StudentSignUp();
                }
                else if(Check_Result == true && Distinguish_Identity == 1)
                {
                    WorkerSignUp();
                }
            }
        }

        private async void StudentSignUp()
        {
            TResponse<Nothing> signUpArgs = null;
            try
            {
                ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
                signUpService.SettingHttpRequest(ServerAddress);

                signUpArgs = await signUpService.Student_SignUp(InputEmail, InputPw, InputName, InputPhone, InputBirth_Year, InputSchool_Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                signUpArgs = null;
            }
            OnStudentSignUpResultRecieved?.Invoke(signUpArgs);
        }

        private async void WorkerSignUp()
        {
            if(InputPw.Length >= 8 && InputPw.Length <= 20)
            {
                Pw_Desc = "사용가능한 비밀번호 입니다.";
                Pw_Desc_Foreground = Brushes.LightGreen;

                TResponse<Nothing> signUpArgs = null;
                try
                {
                    ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
                    signUpService.SettingHttpRequest(ServerAddress);

                    signUpArgs = await signUpService.Worker_SignUp(InputEmail, InputPw, InputName, InputPhone, InputBirth_Year, InputWork_Place, InputWork_Category);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    signUpArgs = null;
                }
                OnWorkerSignUpResultReceived?.Invoke(signUpArgs);
            }
            else
            {
                Pw_Desc = "비밀번호 자리수를 확인해 주세요.";
                Pw_Desc_Foreground = Brushes.Red;
            }
        }

        #region 휴대전화번호 관련 검증 메소드들
        // 전화번호 입력시 자동으로 하이픈 입력
        private void AutoInput_Hyphen(string phoneNumber)
        {
            string phone = phoneNumber;
            string str_phoneNumHyphen;
            string[] phoneNumSplit = new string[3];

            if (phone.Length == 10)
            {
                phoneNumSplit[0] = phone.Substring(0, 3);
                phoneNumSplit[1] = phone.Substring(3, 3);
                phoneNumSplit[2] = phone.Substring(6, 4);
            }
            else
            {
                phoneNumSplit[0] = phone.Substring(0, 3);
                phoneNumSplit[1] = phone.Substring(3, 4);
                phoneNumSplit[2] = phone.Substring(7, 4);

                str_phoneNumHyphen = phoneNumSplit[0] + "-" + phoneNumSplit[1] + "-" + phoneNumSplit[2];

                phoneNumber = str_phoneNumHyphen;
                InputPhone = phoneNumber;
            }

            return;
        }

        // 하이픈 없이 입력할 경우 휴대전화 번호 정규식
        private void PhoneNumRegularExpressionCheck(string phoneNumber)
        {
            string phone = phoneNumber;
            if(phone.Length == 10 || phone.Length == 11)
            {
                Regex regex = new Regex(@"01{1}[016789]{1}[0-9]{7,8}");

                Match match = regex.Match(phone);
                if(match.Success)
                {
                    Check_Result = true;
                }
                else
                {
                    MessageBox.Show("휴대전화 번호를 다시 확인해 주세요.");
                }
            }
            else
            {
                MessageBox.Show("휴대전화 자릿수가 맞지 않습니다.");
            }

            return;
        }

        // 하이픈 있게 입력할 경우 휴대전화 번호 정규식
        private void HypenPhoneNumRegularExpressionCheck(string phoneNumber)
        {
            string phone = phoneNumber;
            if (phone.Length == 12 || phone.Length == 13)
            {
                Regex regex = new Regex(@"01{1}[016789]{1}[0-9]{7,8}");

                Match match = regex.Match(phone);
                if (match.Success)
                {
                    Check_Result = true;
                }
                else
                {
                    MessageBox.Show("휴대전화 번호를 다시 확인해 주세요.");
                }
            }
            else
            {
                MessageBox.Show("휴대전화 자릿수가 맞지 않습니다.");
            }

            return;
        }
        #endregion
        #endregion

        #region 학교 목록 조회 Command
        private bool CanSearchSchool()
        {
            return (InputSchool_Name != null) && (InputSchool_Name != "") && (InputSchool_Name != string.Empty);
        }

        private void OnSearchSchool()
        {
            SearchSchool();
        }

        private async void SearchSchool()
        {
            SchoolItems.Clear();

            ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
            signUpService.SettingHttpRequest(ServerAddress);

            var resp = await signUpService.GetSchoolList(InputSchool_Name);

            if (resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.School schoolInfo = new Model.School();

                    foreach (var item in resp.Data.Schools)
                    {
                        schoolInfo.School_Id = item.School_Id;
                        schoolInfo.Office_Id = item.Office_Id;
                        schoolInfo.School_Name = item.School_Name;
                        schoolInfo.School_Location = item.School_Location;

                        SchoolItems.Add((Model.School)item.Clone());
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }
        #endregion

        #region 이메일 중복 확인 Command
        private bool CanCheckEmailOverLap()
        {
            return (InputEmail != null) && (InputEmail != "") && (InputEmail != string.Empty);
        }

        private void OnCheckEmailOverLap()
        {
            CheckEmailOverLap();
        }

        private async void CheckEmailOverLap()
        {
            try
            {
                ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
                signUpService.SettingHttpRequest(ServerAddress);

                var resp = await signUpService.Check_EmailOverLap(InputEmail);

                if(resp.Status == (int)HttpStatusCode.Conflict)
                {
                    Email_Desc = "중복된 이메일 주소 입니다.";
                    Email_Desc_Foreground = Brushes.Red;
                    Check_Email_OverLap = false;
                }
                else
                {
                    Email_Desc = "사용가능한 이메일 주소 입니다.";
                    Email_Desc_Foreground = Brushes.LightGreen;
                    Check_Email_OverLap = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
        #endregion
    }
}
