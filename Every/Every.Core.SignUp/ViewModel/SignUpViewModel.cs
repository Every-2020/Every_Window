using Every.Core.SignUp.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Input;
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
        #endregion
        #endregion

        // 생성자
        public SignUpViewModel()
        {
            StudentSignUpCommand = new DelegateCommand(OnStudentSignUp, CanSignUp).ObservesProperty(() => InputEmail)
                                                                                  .ObservesProperty(() => InputPw)
                                                                                  .ObservesProperty(() => InputName)
                                                                                  .ObservesProperty(() => InputPhone)
                                                                                  .ObservesProperty(() => InputBirth_Year)
                                                                                  .ObservesProperty(() => InputSchool_Id);
            WorkerSignUpCommand = new DelegateCommand(OnWorkerSignUp, CanSignUp).ObservesProperty(() =>InputEmail)
                                                                                .ObservesProperty(() => InputPw)
                                                                                .ObservesProperty(() => InputName)
                                                                                .ObservesProperty(() => InputPhone)
                                                                                .ObservesProperty(() => InputBirth_Year)
                                                                                .ObservesProperty(() => InputWork_Place)
                                                                                .ObservesProperty(() => InputWork_Category);
            SearchSchoolCommand = new DelegateCommand(OnSearchSchool, CanSearchSchool).ObservesProperty(() => InputSchool_Name);
        }

        // 회원가입시 필요한 정보가 모두 입력되어있는지 확인
        private bool CanSignUp()
        {
            return (InputSchool_Id != null) && (InputSchool_Id != "") && (InputSchool_Id != string.Empty);

            //InputPw != null && InputName != null && InputPhone != null && InputBirth_Year != null && InputSchool_Id != null;
        }

        // SignUpCommand
        private void OnStudentSignUp()
        {
            StudentSignUp();
        }

        private void OnWorkerSignUp()
        {
            WorkerSignUp();
        }

        private bool CanSearchSchool()
        {
            return (InputSchool_Name != null) && (InputSchool_Name != "") && (InputSchool_Name != string.Empty);
        }

        private void OnSearchSchool()
        {
            SearchSchool();
        }

        public void phoneNumHyphen(string phoneNumber)
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

        private async void StudentSignUp()
        {
            phoneNumHyphen(InputPhone);

            TResponse<Nothing> signUpArgs = null;
            try
            {
                ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
                signUpService.SettingHttpRequest(ServerAddress);

                signUpArgs = await signUpService.Student_SignUp(InputEmail, InputPw, InputName, InputPhone, InputBirth_Year, InputSchool_Id);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                signUpArgs = null;
            }

            OnStudentSignUpResultRecieved?.Invoke(signUpArgs);
        }

        private async void WorkerSignUp()
        {
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

        private async void SearchSchool()
        {
            SchoolItems.Clear();

            ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
            signUpService.SettingHttpRequest(ServerAddress);

            var resp = await signUpService.GetSchoolList(InputSchool_Name);

            if(resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.School schoolInfo = new Model.School();

                    foreach(var item in resp.Data.Schools)
                    {
                        schoolInfo.School_Id = item.School_Id;
                        schoolInfo.Office_Id = item.Office_Id;
                        schoolInfo.School_Name = item.School_Name;
                        schoolInfo.School_Location = item.School_Location;

                        SchoolItems.Add((Model.School)item.Clone());
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
