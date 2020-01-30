using BIND.Core.Login;
using Every.Common;
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

        public App()
        {
            Setting.Load();
        }
    }
}
