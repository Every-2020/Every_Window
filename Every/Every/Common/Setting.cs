using Every.Properties;
using Every_AdminWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Common
{
    public class Setting
    {
        public static bool isHostServer { get; set; }
        public static string ServerURL { get; set; }
        public static bool IsAutoLogin { get; set; }

        public static void Load()
        {
            isHostServer = Settings.Default.isHostServer;
            IsAutoLogin = Settings.Default.isAutoLogin;
            //ServerURL = Settings.Default.ServerURL;
            App.loginData.loginViewModel.ServerAddress = Settings.Default.ServerURL;
        }

        public static void Save()
        {
            Settings.Default.isHostServer = isHostServer;
            Settings.Default.isAutoLogin = IsAutoLogin;
            Settings.Default.ServerURL = ServerURL;
            Settings.Default.Save();
        }

        public static void Reset()
        {
            IsAutoLogin = false;
            Save();
        }

        public static void SaveUserData(string username)
        {
            Settings.Default.userId = username;
            Settings.Default.userPw = string.Empty;
            Save();
        }

        public static void SaveUserdata(string username, string password)
        {
            Settings.Default.userId = username;
            Settings.Default.userPw = password;
            Save();
        }

        public static String GetUserId()
        {
            return Settings.Default.userId;
        }

        public static String GetUserPw()
        {
            return Settings.Default.userPw;
        }
    }
}
