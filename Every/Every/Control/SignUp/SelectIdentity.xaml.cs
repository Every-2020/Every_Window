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
    /// Interaction logic for SelectIdentity.xaml
    /// </summary>
    public partial class SelectIdentity : UserControl
    {
        public delegate void BackWardLoginPage_Handler(object sender, RoutedEventArgs e);
        public event BackWardLoginPage_Handler SelectIdentityBackWardLoginPage;

        public delegate void OnCreateStudentAccount_Received_Handler(object sender, RoutedEventArgs e);
        public event OnCreateStudentAccount_Received_Handler OnCreateStudentAccount;

        public delegate void OnCreateWorkerAccount_Received_Handler(object sender, RoutedEventArgs e);
        public event OnCreateWorkerAccount_Received_Handler OnCreateWorkerAccount;

        public SelectIdentity()
        {
            InitializeComponent();
        }

        private void btnCreateStudentAccount_Click(object sender, RoutedEventArgs e)
        {
            OnCreateStudentAccount?.Invoke(this, e);
        }

        private void btnCreateWorkerAccount_Click(object sender, RoutedEventArgs e)
        {
            OnCreateWorkerAccount?.Invoke(this, e);
        }

        private void btnBackWardLoginPage_Click(object sender, RoutedEventArgs e)
        {
            SelectIdentityBackWardLoginPage?.Invoke(this, e);
        }
    }
}
