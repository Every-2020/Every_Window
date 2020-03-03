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

namespace Every.Control.Bamboo
{
    /// <summary>
    /// Interaction logic for BambooControl.xaml
    /// </summary>
    public partial class BambooControl : UserControl, IPage
    {
        private const string Title = "대나무숲";

        public delegate void LoadedBambooPostWindow_Handler(object sender, RoutedEventArgs e);
        public event LoadedBambooPostWindow_Handler OnLoadedBambooPostWindow;

        public BambooControl()
        {
            InitializeComponent();
            this.DataContext = App.bambooData.bambooViewModel;
        }

        public string GetTitle()
        {
            return Title;
        }

        public async void LoadData()
        {
            await App.bambooData.LoadDataAsync();
        }

        private void btnBambooPostWindow_Click(object sender, RoutedEventArgs e)
        {
            OnLoadedBambooPostWindow?.Invoke(this, e);
        }
    }
}