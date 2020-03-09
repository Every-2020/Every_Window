﻿using Every_AdminWin;
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

        private void btnBambooPostWindow_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBambooPostWindow?.Invoke(this, e);
        }

        private void btnBambooReplyCount_Click(object sender, RoutedEventArgs e)
        {
            OnLoadBambooPostWithReply?.Invoke(this, e);
        }
    }
}