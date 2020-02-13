using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.SignUp.Model
{
    public class Duty : BindableBase
    {
        private string _duty_Name;
        public string Duty_Name
        {
            get => _duty_Name;
            set
            {
                SetProperty(ref _duty_Name, value);
            }
        }
    }
}
