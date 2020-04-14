using Every.Common;
using Every.View;
using Every_AdminWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Every.Control
{
    /// <summary>
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        List<NaviData> naviDatas = new List<NaviData>();

        public NavigationControl()
        {
            InitializeComponent();
            Loaded += NavigationControl_Loaded;

            ctrlBamboo.OnLoadBambooPostWithReply += CtrlBamboo_OnLoadBambooPostWithReply;
            ctrlBamboo.OnLoadBambooPostWindow += CtrlBamboo_OnLoadedBambooPostWindow;
        }

        private void NavigationControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.bambooData.bambooViewModel;

            naviDatas.Add(new NaviData
            {
                Title = "메인",
                NaviImagePath = ComDef.Path + "HomeIcon.png",
                naviMenu = NaviMenu.Home
            });
            naviDatas.Add(new NaviData
            {
                Title = "대나무숲",
                NaviImagePath = ComDef.Path + "BambooIcon.png",
                naviMenu = NaviMenu.Bamboo
            }) ;
            naviDatas.Add(new NaviData
            {
                Title = "일정 관리",
                NaviImagePath = ComDef.Path + "ScheduleIcon.png",
                naviMenu = NaviMenu.Schedule
            });
            naviDatas.Add(new NaviData
            {
                Title = "설정",
                NaviImagePath = ComDef.Path + "OptionIcon.png",
                naviMenu = NaviMenu.Option
            });

            lvNavi.ItemsSource = naviDatas;
        }

        private void lvNavi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IPage page = null;
            NaviData selectdata = lvNavi.SelectedItem as NaviData;

            ICollectionView view = CollectionViewSource.GetDefaultView(naviDatas); // ListView의 ItemSource Update

            switch (selectdata.naviMenu)
            {
                case NaviMenu.Home:
                     page = ctrlHome;
                     naviDatas[0].NaviImagePath = ComDef.Path + "ColorHome.png"; // Icon Update
                     naviDatas[1].NaviImagePath = ComDef.Path + "BambooIcon.png";
                     naviDatas[2].NaviImagePath = ComDef.Path + "ScheduleIcon.png";
                     naviDatas[3].NaviImagePath = ComDef.Path + "OptionIcon.png";
                     view.Refresh();
                     // ctrlHome.LoadData();
                     break;
                case NaviMenu.Bamboo:
                     page = ctrlBamboo;
                     // 순서 중요
                     if(App.bambooData.bambooViewModel.PostsItems.Count > 0)
                     { 
                        App.bambooData.bambooViewModel.PostsItems.Clear();
                     }
                     ctrlBamboo.LoadData();
                     naviDatas[0].NaviImagePath = ComDef.Path + "HomeIcon.png";
                     naviDatas[1].NaviImagePath = ComDef.Path + "ColorBamboo.png"; // Icon Update
                     naviDatas[2].NaviImagePath = ComDef.Path + "ScheduleIcon.png";
                     naviDatas[3].NaviImagePath = ComDef.Path + "OptionIcon.png";
                     view.Refresh();
                     break;
                case NaviMenu.Schedule:
                     page = ctrlSchedule;
                     naviDatas[0].NaviImagePath = ComDef.Path + "HomeIcon.png";
                     naviDatas[1].NaviImagePath = ComDef.Path + "BambooIcon.png";
                     naviDatas[2].NaviImagePath = ComDef.Path + "ColorSchedule.png"; // Icon Update
                     naviDatas[3].NaviImagePath = ComDef.Path + "OptionIcon.png";
                     view.Refresh();
                     break;
                case NaviMenu.Option:
                     page = ctrlOption;
                     naviDatas[0].NaviImagePath = ComDef.Path + "HomeIcon.png";
                     naviDatas[1].NaviImagePath = ComDef.Path + "BambooIcon.png";
                     naviDatas[2].NaviImagePath = ComDef.Path + "ScheduleIcon.png";
                     naviDatas[3].NaviImagePath = ComDef.Path + "ColorOption.png"; // Icon Update
                     view.Refresh();
                     break;
            }
            ShowPage(page);
        }

        public void InitView()
        {
            naviDatas[0].NaviImagePath = ComDef.Path + "ColorHome.png"; // Icon Update
            ShowPage(ctrlHome);
        }

        private void ShowPage(IPage page)
        {
            if (page != null && page is FrameworkElement element)
            {
                CollapsePages();
                element.Visibility = Visibility.Visible;
            }

            if (page == ctrlBamboo)
            {
                tbBambooSearchPost.Visibility = Visibility.Visible; // 게시글 검색바 Visible
            }
            else
            {
                tbBambooSearchPost.Visibility = Visibility.Collapsed; // 게시글 검색바 Collapsed
            }
        }

        private void CollapsePages()
        {
            foreach (FrameworkElement element in gdPage.Children)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        // Post with Reply
        private async void CtrlBamboo_OnLoadBambooPostWithReply(object sender, RoutedEventArgs e)
        {
            int idx = Convert.ToInt32(sender);
            await App.bambooData.bambooViewModel.GetPost(idx);
            await App.bambooData.bambooViewModel.GetReplies(idx);

            BambooPostWithReply bambooPostWithReply = new BambooPostWithReply();
            bambooPostWithReply.DataContext = App.bambooData.bambooViewModel;
            bambooPostWithReply.ShowDialog(); // MainWindow Focus 이동 불가, 즉 모달 창
            //bambooPostWithReply.Show(); // MainWindow Focus 이동 가능
        }

        // Create Post
        private void CtrlBamboo_OnLoadedBambooPostWindow(object sender, RoutedEventArgs e)
        {
            BambooPostWindow bambooPostWindow = new BambooPostWindow();
            bambooPostWindow.ModalBackGroundVisibility += BambooPostWindow_ModalBackGroundVisibility; // 게시물 작성 취소

            ncModalBackGround.Visibility = Visibility.Visible; // 부모 윈도우 Modal 창 Visible

            bambooPostWindow.ShowDialog(); // MainWindow Focus 이동 불가, 즉 모달 창
            //bambooPostWindow.Show(); // MainWindow Focus 이동 가능
        }

        private void BambooPostWindow_ModalBackGroundVisibility()
        {
            ncModalBackGround.Visibility = Visibility.Collapsed;
        }
    }

    public class NaviData
    {
        public NaviMenu naviMenu { get; set; }
        public string Title { get; set; }
        public string NaviImagePath { get; set; }
    }

    public enum NaviMenu
    {
        Home, Bamboo, Schedule, Option
    }

    interface IPage
    {
        string GetTitle();
    }
}
