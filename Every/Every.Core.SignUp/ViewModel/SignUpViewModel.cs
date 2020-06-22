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

        #region Properties
        // 학생 회원가입 후 연결되는 델리게이트 & 이벤트
        public delegate void OnStudentSignUpResultReceivedHandler(TResponse<Nothing> signUpArgs);
        public event OnStudentSignUpResultReceivedHandler OnStudentSignUpResultRecieved;

        // 직장인 or 대학생 회원가입 후 연결되는 델리게이트 & 이벤트
        public delegate void OnWorkerSignUpResultReceivedHandler(TResponse<Nothing> signUpArgs);
        public event OnWorkerSignUpResultReceivedHandler OnWorkerSignUpResultReceived;

        // 전화번호 중복 확인 여부를 위한 변수
        private bool CheckPhoneNum_OverLap = false;
        // 전화번호 정규식 검증 결과 여부 확인을 위한 변수
        private bool CheckPhone_RegularExpression;

        // 이메일 중복 확인을 위한 변수
        private bool CheckEmail_OverLap = false;
        // 이메일 정규식 검증 결과 여부 확인을 위한 변수
        private bool CheckEmail_RegularExpression = false;
        
        // 회원가입전 신분 선택시 학생, 직장인 or 대학생을 구분하기 위한 변수(0 : 학생, 1 : 직장인 or 대학생)
        public int Distinguish_Identity; 

        #region 학생 & 직장인 공통 Properties
        private string _inputEmail;
        public string InputEmail
        {
            get => _inputEmail;
            set
            {
                SetProperty(ref _inputEmail, value);
                
                // 이메일 입력시 자동으로 정규식 검사
                CheckEmailOverLap();
            }
        }

        private string _inputPw;
        public string InputPw
        {
            get => _inputPw;
            set
            {
                SetProperty(ref _inputPw, value);

                // 비밀번호 입력시 자동으로 자릿수 검사 
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

        private string _inputPhoneNum;
        public string InputPhoneNum
        {
            get => _inputPhoneNum;
            set
            {
                SetProperty(ref _inputPhoneNum, value);

                // 휴대전화번호 입력시 자동으로 정규식 검사
                CheckPhoneNumRegularExpression(Distinguish_Identity);

                // 휴대전화번호 중복 확인
                CheckPhoneNumOverLap();
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

        // 이메일 Validation 정보
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

        // 비밀번호 Validation 정보
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

        // 전화번호 Validation 정보
        private string _phoneNum_Desc;
        public string PhoneNum_Desc
        {
            get => _phoneNum_Desc;
            set
            {
                SetProperty(ref _phoneNum_Desc, value);
            }
        }
        private System.Windows.Media.Brush _phoneNum_Desc_Foreground;
        public System.Windows.Media.Brush PhoneNum_Desc_Foreground
        {
            get => _phoneNum_Desc_Foreground;
            set
            {
                SetProperty(ref _phoneNum_Desc_Foreground, value);
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

        private Model.School _selectedSchool = new Model.School();
        public Model.School SelectedSchool
        {
            get => _selectedSchool;
            set
            {
                SetProperty(ref _selectedSchool, value);

                // 선택된 학교의 학교코드를 자동으로 넣어줌
                InputSchool_Id = value.School_Id.ToString();
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
                Check_BlankSpace();
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

        // 업무 분야
        public ObservableCollection<Model.Duty> DutyItems { get; set; }

        // 선택한 직종
        private Model.Duty _selectedDuty = new Model.Duty();
        public Model.Duty SelectedDuty
        {
            get => _selectedDuty;
            set
            {
                SetProperty(ref _selectedDuty, value);
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

        // Command 연속클릭 방지
        private bool _isEnable = true;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }

        // StudentSignUpControl Modal Background
        public Visibility ModalBackGround { get; set; } = Visibility.Collapsed;

        #region Commands

        // 학생 회원가입 Command
        public ICommand StudentSignUpCommand { get; set; }
        // 직장인 or 대학생 회원가입 Command
        public ICommand WorkerSignUpCommand { get; set; }
        // 학교 검색 Command
        public ICommand SearchSchoolCommand { get; set; }
        // 이메일 중복확인 Command, 현재 사용 X,  자동 처리
        public ICommand CheckEmailOverLapCommand { get; set; }
        #endregion
        #endregion

        // 생성자
        public SignUpViewModel()
        {
            LoadDuties();

            StudentSignUpCommand = new DelegateCommand(OnStudentSignUp, CanStudentSignUp).ObservesProperty(() => InputSchool_Id);
            WorkerSignUpCommand = new DelegateCommand(OnWorkerSignUp, CanWorkerSignUp).ObservesProperty(() => InputWork_Category);
            SearchSchoolCommand = new DelegateCommand(OnSearchSchool, CanSearchSchool).ObservesProperty(() => InputSchool_Name);
        }

        private void LoadDuties()
        {
            ObservableCollection<Model.Duty> duties = new ObservableCollection<Model.Duty>();
            #region 직무 분야
            duties.Add(new Model.Duty 
            { 
                Duty_Name = "1. IT·인터넷" 
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "2. 영업·고객상담"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "3. 생산·제조"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "4. 의료"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "5. 경영·사무"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "6. 디자인"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "7. 연구개발·설계"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "8. 교육"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "9. 미디어"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "10. 마케팅·광고·홍보"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "11. 무역·유통"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "12. 서비스"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "13. 건설"
            });

            duties.Add(new Model.Duty
            {
                Duty_Name = "14. 전문·특수직"
            });
#endregion
            DutyItems = duties;
        }

        #region 회원가입
        private bool CanStudentSignUp()
        {
            return (InputSchool_Id != null) && (InputSchool_Id != "") && (InputSchool_Id != string.Empty);
        }
        private bool CanWorkerSignUp()
        {
            return (InputWork_Category != null) && (InputWork_Category.ToString() != "") && (InputWork_Category.ToString() != string.Empty);
        }

        private void OnStudentSignUp()
        {
            if(CheckEmail_OverLap == true && CheckEmail_RegularExpression == true &&
               CheckPhoneNum_OverLap == true && CheckPhone_RegularExpression == true)
            {
                SignUp();
            }
        }
        private void OnWorkerSignUp()
        {
            if(CheckEmail_OverLap == true && CheckEmail_RegularExpression == true &&
               CheckPhoneNum_OverLap == true && CheckPhone_RegularExpression == true)
            {
                SignUp();
            }
        }

        private void CheckPhoneNumRegularExpression(int Distinguish_Identity)
        {
            // 휴대전화 입력시 하이픈("-")을 입력한 경우
            if (InputPhoneNum.Contains("-"))
            {
                // 하이픈이 있는 휴대전화 번호의 정규식 확인
                IsValidHypenPhoneNum(InputPhoneNum);
            }

            // 휴대전화 입력시 하이픈("-")을 입력하지 않은 경우
            else
            {
                // 하이픈이 없는 휴대전화 번호의 정규식 확인
                IsValidPhoneNum(InputPhoneNum);

                // 정규식 확인 후 자동으로 하이픈 입력
                AutoInput_Hyphen(InputPhoneNum);
            }
        }

        private void SignUp()
        {
            IsEnable = false;

            if (Distinguish_Identity == 0)
            {
                StudentSignUp();
            }
            else if (Distinguish_Identity == 1)
            {
                WorkerSignUp();
            }

            IsEnable = true;
        }

        private async void StudentSignUp()
        {
            TResponse<Nothing> signUpArgs = null;
            try
            {
                ServerAddress = "ServerAddress";
                signUpService.SettingHttpRequest(ServerAddress);

                signUpArgs = await signUpService.Student_SignUp(InputEmail, InputPw, InputName, InputPhoneNum, InputBirth_Year, InputSchool_Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("StudentSignUp Error : " + e.Message);
                signUpArgs = null;
            }
            OnStudentSignUpResultRecieved?.Invoke(signUpArgs);
        }

        private async void WorkerSignUp()
        {
            TResponse<Nothing> signUpArgs = null;
            try
            {
                ServerAddress = "ServerAddress";
                signUpService.SettingHttpRequest(ServerAddress);

                signUpArgs = await signUpService.Worker_SignUp(InputEmail, InputPw, InputName, InputPhoneNum, InputBirth_Year, InputWork_Place, InputWork_Category);
            }
            catch (Exception e)
            {
                Debug.WriteLine("WorkerSignUp Error : " + e.Message);
                signUpArgs = null;
            }
            OnWorkerSignUpResultReceived?.Invoke(signUpArgs);
        }
        #endregion

        #region 전화번호 정규식 검사 & 자동 하이픈 입력

        // 전화번호 입력시 자동 하이픈 입력
        private void AutoInput_Hyphen(string phoneNumber)
        {
            string phone = phoneNumber;
            string[] phoneNumSplit = new string[3];
            string str_phoneNumHyphen;

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
                InputPhoneNum = phoneNumber;
            }

            return;
        }

        // 하이픈 없이 입력할 경우 휴대전화 번호 정규식 검사
        private void IsValidPhoneNum(string phoneNumber)
        {
            string phone = phoneNumber;
            if (phone.Length == 10 || phone.Length == 11)
            {
                Regex regex = new Regex(@"01{1}[016789]{1}[0-9]{7,8}");

                Match match = regex.Match(phone);
                if (match.Success)
                {
                    CheckPhone_RegularExpression = true;
                    PhoneNum_Desc = "올바른 전화번호입니다.";
                    PhoneNum_Desc_Foreground = Brushes.LightGreen;
                }
                else
                {
                    CheckPhone_RegularExpression = false;
                    PhoneNum_Desc = "휴대전화 번호를 다시 확인해 주세요.";
                    PhoneNum_Desc_Foreground = Brushes.Red;
                }
            }
            else
            {
                CheckPhone_RegularExpression = false;
                PhoneNum_Desc = "휴대전화 자릿수가 맞지 않습니다.";
                PhoneNum_Desc_Foreground = Brushes.Red;
            }

            return;
        }

        // 하이픈 있게 입력할 경우 휴대전화 번호 정규식 검사
        private void IsValidHypenPhoneNum(string phoneNumber)
        {
            string phone = phoneNumber;
            if (phone.Length == 12 || phone.Length == 13)
            {
                Regex regex = new Regex(@"01{1}[016789]{1}-[0-9]{3,4}-[0-9]{4}");

                Match match = regex.Match(phone);
                if (match.Success)
                {
                    CheckPhone_RegularExpression = true;
                    PhoneNum_Desc = "올바른 전화번호입니다.";
                    PhoneNum_Desc_Foreground = Brushes.LightGreen;
                }
                else
                {
                    CheckPhone_RegularExpression = false;
                    PhoneNum_Desc = "휴대전화 번호를 다시 확인해 주세요.";
                    PhoneNum_Desc_Foreground = Brushes.Red;
                }
            }
            else
            {
                CheckPhone_RegularExpression = false;
                PhoneNum_Desc = "휴대전화 자릿수가 맞지 않습니다.";
                PhoneNum_Desc_Foreground = Brushes.Red;
            }

            return;
        }

        private async void CheckPhoneNumOverLap()
        {
            if (CheckPhone_RegularExpression == true)
            {
                try
                {
                    ServerAddress = "http://49.50.160.97:8080";
                    signUpService.SettingHttpRequest(ServerAddress);

                    var resp = await signUpService.Check_PhoneNumOverLap(InputPhoneNum);

                    if (resp.Status == (int)HttpStatusCode.Conflict)
                    {
                        PhoneNum_Desc = "중복된 전화번호 입니다.";
                        PhoneNum_Desc_Foreground = Brushes.Red;
                        CheckPhoneNum_OverLap = false;
                    }
                    else if (resp.Status == (int)HttpStatusCode.OK)
                    {
                        PhoneNum_Desc = "사용가능한 전화번호 입니다.";
                        PhoneNum_Desc_Foreground = Brushes.LightGreen;
                        CheckPhoneNum_OverLap = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }
        #endregion

        #region 이메일 중복 확인 
        // 이메일 정규식 검사
        private void IsValidEmailAddress(string emailAddress)
        {
            string email = emailAddress;

            Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            Match match = regex.Match(email);

            if (match.Success)
            {
                CheckEmail_RegularExpression = true;
            }
            else
            {
                Email_Desc = "잘못된 이메일 형식입니다.";
                Email_Desc_Foreground = Brushes.Red;
                CheckEmail_RegularExpression = false;
            }
        }

        //RegexOptions.IgnoreCase : 대 / 소문자를 구분하지 않고 일치 항목을 찾도록 지정.

        private async void CheckEmailOverLap()
        {
            IsValidEmailAddress(InputEmail);

            if (CheckEmail_RegularExpression == true)
            {
                try
                {
                    ServerAddress = "http://49.50.160.97:8080";
                    signUpService.SettingHttpRequest(ServerAddress);

                    var resp = await signUpService.Check_EmailOverLap(InputEmail);

                    if (resp.Status == (int)HttpStatusCode.Conflict)
                    {
                        Email_Desc = "중복된 이메일 주소 입니다.";
                        Email_Desc_Foreground = Brushes.Red;
                        CheckEmail_OverLap = false;
                    }
                    else
                    {
                        Email_Desc = "사용가능한 이메일 주소 입니다.";
                        Email_Desc_Foreground = Brushes.LightGreen;
                        CheckEmail_OverLap = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }
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
            IsEnable = false;

            if(SchoolItems.Count > 0)
            {
                SchoolItems.Clear();
            }

            ServerAddress = "http://49.50.160.97:8080";
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
                    Debug.WriteLine("SearchSchool Error : " + e.Message);
                }
            }

            IsEnable = true;
        }
        #endregion

        #region InputWork_Place BlankSpace Validate
        private void Check_BlankSpace()
        {
            if(InputWork_Place.Contains(" "))
            {
                InputWork_Place = InputWork_Place.Replace(" ", "");
            }
            return;
        }
        #endregion
    }
}
