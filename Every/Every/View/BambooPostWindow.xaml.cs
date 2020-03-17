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
using System.Windows.Shapes;

namespace Every.View
{
    /// <summary>
    /// Interaction logic for BambooPostWindow.xaml
    /// </summary>
    public partial class BambooPostWindow : Window
    {
        public delegate void OnModalBackgroundVisibility();
        public event OnModalBackgroundVisibility ModalBackGroundVisibility;
        public BambooPostWindow()
        {
            InitializeComponent();
            Loaded += BambooPostWindow_Loaded;
        }

        private void BambooPostWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bambooData.bambooViewModel;
            App.bambooData.bambooViewModel.BambooPostResultReceived += BambooViewModel_BambooPostResultReceived;
        }

        private void BambooViewModel_BambooPostResultReceived(object sender)
        {
            this.Close();
            ModalBackGroundVisibility?.Invoke();
        }

        private void btn_CloseBambooPostWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ModalBackGroundVisibility?.Invoke();
        }
    }
}
