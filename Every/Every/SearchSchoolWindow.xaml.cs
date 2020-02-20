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

        // ListView SelectionChanged Event 
#if false
        SelectionChanged="ListView_SelectionChanged"
#endif
        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = ((School)(sender as ListView).SelectedItem).School_Id.ToString();
        //    if(App.signUpData.signUpViewModel.InputSchool_Id != item)
        //    {
        //        App.signUpData.signUpViewModel.InputSchool_Id = item;
        //        this.Close();
        //    }
        //}
    }
}
