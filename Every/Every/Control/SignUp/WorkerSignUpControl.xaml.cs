using Every.Core.SignUp.Model;
using Every_AdminWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WorkerSignUp.xaml
    /// </summary>
    public partial class WorkerSignUp : UserControl
    {
        public delegate void BackWardLoginPage_Handler(object sender, RoutedEventArgs e);
        public event BackWardLoginPage_Handler WorkerSignUpBackWardLoginPage;

        public WorkerSignUp()
        {
            InitializeComponent();
            this.DataContext = App.signUpData.signUpViewModel;
        }

        private void btnBackWardLoginPage_Click(object sender, RoutedEventArgs e)
        {
            WorkerSignUpBackWardLoginPage?.Invoke(this, e);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((Duty)(sender as ComboBox).SelectedItem).Duty_Name;

            var quantity = App.signUpData.signUpViewModel.DutyItems;

            for (int i = 0; i < quantity.Count; i++)
            {
                if (item == quantity[i].Duty_Name && i < 9)
                {
                    App.signUpData.signUpViewModel.InputWork_Category = Convert.ToInt32(item.Substring(0, 1));
                }

                if (item == quantity[i].Duty_Name && i >= 9)
                {
                    App.signUpData.signUpViewModel.InputWork_Category = Convert.ToInt32(item.Substring(0, 2));
                }
            }
        }

        // 숫자만 입력 가능, 키패드 사용 가능
        private void tbInputBirth_Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
