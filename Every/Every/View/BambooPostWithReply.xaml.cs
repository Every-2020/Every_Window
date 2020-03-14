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
    /// Interaction logic for BambooPostWithReply.xaml
    /// </summary>
    public partial class BambooPostWithReply : Window
    {
        public BambooPostWithReply()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                this.DataContext = App.bambooData.bambooViewModel;
            };
        }

        private void btn_CloseBambooPostWithReply_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
