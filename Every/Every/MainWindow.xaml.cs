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

namespace Every_AdminWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginCtrl_OnLoginResultRecieved(object sender, bool success)
        {
            if (success)
            {
                LoginCtrl.Visibility = Visibility.Collapsed;
                MessageBox.Show("로그인에 성공하셨습니다!");
            }

        }
    }
}
