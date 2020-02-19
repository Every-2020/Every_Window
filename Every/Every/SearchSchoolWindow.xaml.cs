using Every.Core.SignUp.Model;
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

namespace Every
{
    /// <summary>
    /// Interaction logic for SearchSchoolWindow.xaml
    /// </summary>
    public partial class SearchSchoolWindow : Window
    {
        public SearchSchoolWindow()
        {
            InitializeComponent();
            this.Loaded += SearchSchoolWindow_Loaded;
        }

        private void SearchSchoolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.signUpData.signUpViewModel;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = sender as ListView;
            App.signUpData.signUpViewModel.InputSchool_Id = ((School)selectedItem.SelectedItem).School_Id.ToString();
            
        }
    }
}
