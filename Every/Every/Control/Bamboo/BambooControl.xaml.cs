using Every.Core.Bamboo.Model;
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

        public delegate void LoadBambooPostWindow_Handler(object sender, RoutedEventArgs e);
        public event LoadBambooPostWindow_Handler OnLoadBambooPostWindow;

        public delegate void LoadBambooPostWithReplyWindow_Handler(object sender, RoutedEventArgs e);
        public event LoadBambooPostWithReplyWindow_Handler OnLoadBambooPostWithReply;

        public BambooControl()
        {
            InitializeComponent();
            Loaded += BambooControl_Loaded;
        }

        private void BambooControl_Loaded(object sender, RoutedEventArgs e)
        {
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

        // 게시물 작성 클릭
        private void btnBambooPostWindow_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBambooPostWindow?.Invoke(this, e);
        }

        // 댓글 작성 클릭, TODO : 커맨드로 바꿔야함, 현재는 마땅히 떠오르지 않아서 임시로 나둠.
        private void btnMakeReply_Click(object sender, RoutedEventArgs e)
        {
            string replycontent = App.bambooData.bambooViewModel.BambooReplyContent;
            int? idx = Convert.ToInt32((sender as Button).Tag);
            
            if (idx != null && replycontent != null)
            {
                App.bambooData.bambooViewModel.BambooReply(replycontent, idx);
            }
        }

        // 댓글 개수 클릭
        private void btnBambooReplyCount_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag;
            OnLoadBambooPostWithReply?.Invoke(item, e);
        }
    }
}