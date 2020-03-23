using BIND.Core.Login;
using Every;
using Every.Common;
using Every.Control;
using Every.Control.SignUp;
using Every.Core.Bamboo;
using Every.Core.Meal;
using Every.Core.Member;
using Every.Core.SignUp;
using Every.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Every_AdminWin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Class Library
        public static BambooData bambooData = new BambooData();
        public static LoginData loginData = new LoginData();
        public static MemberData memberData = new MemberData();
        public static SignUpData signUpData = new SignUpData();
        public static MealData mealData = new MealData();

        // View/Window
        public static SelectIdentity selectIdentity = new SelectIdentity();
        public static SearchSchoolWindow searchSchoolWindow = new SearchSchoolWindow();
        public static BambooPostWindow bambooPostWindow = new BambooPostWindow();

        // Start
        public App()
        {
            Setting.Load();
        }

        // WPF 전역 예외처리, 어플리케이션 강제 종료 방지
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occured: " + e.Exception, "예외 발생", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // 이 속성은 예외 처리 완료, 이와 관련해 더이상 아무것도 하지 않아도 된다는 것을 WPF에 전달.
        }
    }
}
