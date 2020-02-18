using Every.Common;
using Every_AdminWin;
using System;
using System.Collections.Generic;
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

namespace Every.Control.SignUp
{
    /// <summary>
    /// Interaction logic for SignUpControl.xaml
    /// </summary>
    public partial class StudentSignUpControl : UserControl
    {
        public delegate void BackWardLoginPage_Handler(object sender, RoutedEventArgs e);
        public event BackWardLoginPage_Handler StudentSignUpBackWardLoginPage;

        public StudentSignUpControl()
        {
            InitializeComponent();
            this.DataContext = App.signUpData.signUpViewModel;
        }

        private void btnBackWardLoginPage_Click(object sender, RoutedEventArgs e)
        {
            StudentSignUpBackWardLoginPage?.Invoke(this, e);
        }
    }
}
