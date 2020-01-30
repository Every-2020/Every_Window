using BIND.Core.Login.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TNetwork.Data;
using Prism.Mvvm;
using Prism.Commands;
using System.Net.NetworkInformation;
using System.Net;

namespace BIND.Core.Login.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        #region Property
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value.Trim());
        }

        private string _desc;
        public string Desc
        {
            get => _desc;
            set => SetProperty(ref _desc, value);
        }

        private bool _btnLoginEnabled;
        public bool BtnLoginEnabled
        {
            get => _btnLoginEnabled;
            set => SetProperty(ref _btnLoginEnabled, value);
        }

        private bool _progressRingActivated;
        public bool ProgressRingActivated
        {
            get => _progressRingActivated;
            set => SetProperty(ref _progressRingActivated, value);
        }
        #endregion

        #region Delegate 
        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler OnLoginResultRecieved;
        #endregion

        #region Command
        public ICommand LoginCommand { get; set; }
        #endregion
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(OnLogin);
            //_btnLoginEnabled = true;
        }

        internal async void OnLogin()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                SendOnLoginResultRecievedEvent(false);
                
                Desc = "네트워크 상태를 확인해주세요!!";
                return;
            }

            BtnLoginEnabled = false;
            ProgressRingActivated = true;

            Debug.WriteLine($"{_btnLoginEnabled} OnLogin");

            TResponse<TokenInfo> loginArgs = null;
            var loginService = new LoginService();
            try
            {
                loginService.SettingHttpRequest(_serverAddress);

                loginArgs = await loginService.Login(Id, Password);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                loginArgs = null;
            }

            if (loginArgs == null || loginArgs.Status != (int)HttpStatusCode.OK)    //로그인 실패
            {
                /*  Setting.IsAutoLogin = false;
                  Setting.Save();
                  MessageBox.Show("로그인 실패", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                  pbPw.Focus();
                  pbPw.Password = string.Empty;
                  */
                SendOnLoginResultRecievedEvent(false);
                Desc = "로그인에 실패하였습니다!";
                Debug.WriteLine(Desc);
            }
            else    //로그인 성공
            {
                SendOnLoginResultRecievedEvent(true);
                Desc = "로그인에 성공하였습니다!";
                Debug.WriteLine(Desc);
            }
            BtnLoginEnabled = true;
            ProgressRingActivated = false;
        }

        private void SendOnLoginResultRecievedEvent(bool success)
        {
            OnLoginResultRecieved?.Invoke(this, success);
        }
    }
}
