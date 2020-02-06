using BIND.Core.Login;
using Every.Common;
using Every.Control.SignUp;
using Every.Core.SignUp;
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
        public static LoginData loginData = new LoginData();
        public static SignUpData signupData = new SignUpData();

        public static SelectIdentity selectIdentity = new SelectIdentity();
        public App()
        {
            Setting.Load();
        }
    }
}
