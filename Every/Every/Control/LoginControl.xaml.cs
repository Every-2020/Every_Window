using Every.Common;
using Every_AdminWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace Every.Control
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public delegate void OnSignUpRecievedHandler(object sender, bool success);
        public event OnSignUpRecievedHandler OnSignUpReceived;

        private bool isAutoLogin = false;

        public delegate void onLoginResultRecievedHandler(object sender, bool success);
        public event onLoginResultRecievedHandler OnLoginResultRecieved;

        public LoginControl()
        {
            InitializeComponent();
            this.Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.loginData.loginViewModel.ServerAddress = "http://ec2-13-209-17-179.ap-northeast-2.compute.amazonaws.com:8080";
            CheckAutoLoginAsync();
            //loginData = new LoginData();
            App.loginData.loginViewModel.OnLoginResultRecieved += LoginViewModel_OnLoginResultRecieved;
            this.DataContext = App.loginData.loginViewModel;
        }

        private void LoginViewModel_OnLoginResultRecieved(object sender, bool success)
        {
            #region 개발 편의상 최근 입력받은 서버 Url을 저장 
            if (success)
            {
                Setting.SaveUserdata(App.loginData.loginViewModel.Id, App.loginData.loginViewModel.Password);
                Setting.Save();
            }
            #endregion

            SetUserData(App.loginData.loginViewModel.Id, App.loginData.loginViewModel.Password);

            if (isAutoLogin == true)
            {
                Setting.IsAutoLogin = true;
            }

            // 로그인 ViewModel의 OnLoginResultRecieved가 끝난 후에야 비로소 MainWindow.xaml의 로그인 컨트롤의 OnLoginResultRecieved 이벤트가 연결된다.
            OnLoginResultRecieved?.Invoke(this, success);
        }

        /// <summary>
        /// 자동로그인을 체크한 경우 자동로그인, 
        /// 그렇지 않은 경우 마지막으로 로그인에 성공한 아이디를 미리 입력
        /// </summary>
        public async Task CheckAutoLoginAsync()
        {
            //Setting.Load(); //chris - app에서 호출
            string id = Setting.GetUserId();
            isAutoLogin = Setting.IsAutoLogin;
            cbAutoLogin.IsChecked = isAutoLogin;
            App.loginData.loginViewModel.Id = id;
            string pw = Setting.GetUserPw();

            pbPw.Password = pw;
            if (isAutoLogin)
            {
                App.loginData.loginViewModel.Password = pw;
                App.loginData.Login();
            }
            else if (!string.IsNullOrEmpty(id))
            {
                pbPw.Focus();
            }
            else
            {
                tbId.Focus();
            }
        }

        /// <summary>
        /// 엔터키 누를 시 로그인 버튼 동작
        /// </summary>
        private void Usercontrol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && BtnLogin.IsEnabled)
            {
                App.loginData.Login();
            }
        }

        /// <summary>
        /// 로그인 폼이 채워졌을 때 로그인 버튼 활성화
        /// </summary>
        private void Tb_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty())
            {
                BtnLogin.IsEnabled = true;
            }
            else
            {
                BtnLogin.IsEnabled = false;
            }
        }

        /// <summary>
        /// 로그인 폼이 비어있는지 체크
        /// </summary>
        /// <returns>아이디 비밀번호가 모두 입력되어있어야 true 반환</returns>
        private bool CheckEmpty()
        {
            string id = tbId.Text.Trim();
            string pw = pbPw.Password.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 자동로그인 체크박스 이벤트
        /// </summary>
        private void CbAutologin_Checked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.IsChecked ?? false)
            {
                isAutoLogin = true;
            }
            else
            {
                isAutoLogin = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#if true //loginData.loginViewmodel에 Binding으로 값이 저장되어있음. 
        /// <summary>
        /// Resource(Setting)에 ID와 PW를 저장.
        /// </summary>
        /// <param name="id">저장할 ID</param>
        /// <param name="pw">저장할 PW</param>
        private void SetUserData(string id, string pw)
        {
            if (isAutoLogin)
            {
                Setting.SaveUserdata(id, pw);
            }
            else
            {
                Setting.SaveUserData(id);
            }
            Setting.IsAutoLogin = isAutoLogin;
            Setting.Save();
        }
#endif

        private void tbSign_Click(object sender, RoutedEventArgs e)
        {
            OnSignUpReceived?.Invoke(this, true);
        }
    }

    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }


    // PasswordBox 워터마크 & 비밀번호 바인딩을 하기위해서 사용.
    public class PasswordHelper : DependencyObject
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}