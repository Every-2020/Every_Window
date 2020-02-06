using BIND.Core.Login.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Data;

namespace BIND.Core.Login
{
    public class LoginData
    {
        public LoginViewModel loginViewModel = new LoginViewModel();

        public void Login()
        {
            loginViewModel.OnLogin();
        }
    }
}
