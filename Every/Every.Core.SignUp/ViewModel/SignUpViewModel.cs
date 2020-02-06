using Every.Core.SignUp.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

        public delegate void OnSignUpResultReceivedHandler(TResponse<Nothing> signUpArgs);
        public event OnSignUpResultReceivedHandler OnSignUpResultRecieved;

        #region Properties
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
            get => _inputEmail;
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

        private string _inputBirth_Year;
        public string InputBirth_Year
        {
            get => _inputBirth_Year;
            set
            {
                SetProperty(ref _inputBirth_Year, value);
            }
        }

        private string _inputSchool_Id;
        public string InputSchool_Id
        {
            get => _inputSchool_Id;
            set
            {
                SetProperty(ref _inputSchool_Id, value);
           }
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value.Trim());
        }

        public ICommand SignUpCommand { get; set; }
        public ICommand CreateStudentCommand { get; set; }
        public ICommand CreateWorkerCommand { get; set; }
#endregion

        // 생성자
        public SignUpViewModel()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp).ObservesProperty(()=>InputEmail);
        }

        // 회원가입시 필요한 정보가 모두 입력되어있는지 확인
        private bool CanSignUp()
        {
            return (InputEmail != null) && (InputEmail != "") && (InputEmail != string.Empty);

            //InputPw != null && InputName != null && InputPhone != null && InputBirth_Year != null && InputSchool_Id != null;
        }

        // SignUpCommand
        private void OnSignUp()
        {
            SignUp();
        }

        // 전화 번호입력시 하이픈 자동 입력
        //public void phoneNumHyphen(string phoneNumber)
        //{
        //    string phone = phoneNumber;
        //    string str_phoneNumHyphen;
        //    string[] phoneNumSplit = new string[3];

        //    if (phone.Length == 10)
        //    {
        //        phoneNumSplit[0] = phone.Substring(0, 3);
        //        phoneNumSplit[1] = phone.Substring(3, 3);
        //        phoneNumSplit[2] = phone.Substring(6, 4);
        //    }
        //    else
        //    {
        //        phoneNumSplit[0] = phone.Substring(0, 3);
        //        phoneNumSplit[1] = phone.Substring(3, 4);
        //        phoneNumSplit[2] = phone.Substring(7, 4);
        //    }

        //    str_phoneNumHyphen = phoneNumSplit[0] + "-" + phoneNumSplit[1] + "-" + phoneNumSplit[2];

        //    phoneNumber = str_phoneNumHyphen;
        //}

        private async void SignUp()
        {
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

            OnSignUpResultRecieved?.Invoke(signUpArgs);
        }
    }
}
