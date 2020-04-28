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
using TNetwork.Common;

namespace Every.Control.Home
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl, IPage
    {
        private const string Title = "홈";

        public HomeControl()
        {
            InitializeComponent();
            Loaded += HomeControl_Loaded;
        }

        private void HomeControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.mealData.mealViewModel;
            tbUserName.DataContext = App.memberData.memberViewModel;
        }

        public string GetTitle()
        {
            return Title;
        }

        public async void LoadData()
        {
            await App.mealData.LoadDataAsync();
        }
    }
}
