using Every_AdminWin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // 특정 게시물에서 댓글 작성 클릭, TODO : 커맨드로 바꿔야함, 현재는 마땅히 떠오르지 않아서 임시로 나둠.
        private void btnMakeReply_Click(object sender, RoutedEventArgs e)
        {
            string replycontent = App.bambooData.bambooViewModel.BambooReplyContent;
            int? idx = Convert.ToInt32((sender as Button).Tag);

            if (idx != null && replycontent != null)
            {
                App.bambooData.bambooViewModel.BambooSpecificPostReply(replycontent, idx);
            }
        }

        private void btn_BambooReplyContextMenu_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }
    }
}
