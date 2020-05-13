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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Every.Control.Option
{
    /// <summary>
    /// Interaction logic for OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl, IPage
    {
        private const string Title = "설정";

        #region Field
        // BitmapImage List
        private List<BitmapImage> bitmapImageList = new List<BitmapImage>();

        // Dispatcher Timer
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        // Current Image Storyboard Start Status
        private bool startCurrentImageStoryboard = false;

        // Current Index
        private int currentIndex = 0;
        #endregion

        public OptionControl()
        {
            InitializeComponent();
            Loaded += OptionControl_Loaded;
        }

        private void OptionControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.memberData.memberViewModel;

            this.dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            this.dispatcherTimer.Tick += DispatcherTimer_Tick;
            this.dispatcherTimer.Start();
            SetBitmapImageList();
        }

        #region DispatcherTimer Tick
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.currentIndex++;

            if(this.currentIndex > this.bitmapImageList.Count - 1)
            {
                this.currentIndex = 0;
            }
            if(this.startCurrentImageStoryboard)
            {
                this.currentImage.Source = this.bitmapImageList[this.currentIndex];
                (Resources["CurrentImageStoryboardKey"] as Storyboard).Begin(this);
            }
            else
            {
                this.nextImage.Source = this.bitmapImageList[this.currentIndex];
                (Resources["NextImageStoryboardKey"] as Storyboard).Begin(this);
            }

            this.startCurrentImageStoryboard = !this.startCurrentImageStoryboard;
        }
        #endregion

        #region 리소스 URI 구하기
        /// <summary>
        /// 리소스 URI 구하기
        /// </summary>
        /// <param name="assemblyName">어셈블리명</param>
        /// <param name="imagePath">리소스 경로</param>
        /// <returns></returns>
        private Uri GetResourceURI(string assemblyName, string imagePath)
        {
            if(string.IsNullOrEmpty(assemblyName))
            {
                return new Uri(string.Format("pack://application:,,,/{0}", imagePath));
            }
            else
            {
                return new Uri(string.Format("pack://application:,,,/{0};component/{1}", assemblyName, imagePath));
            }
        }
        private void SetBitmapImageList()
        {
            this.bitmapImageList.Clear();

            string[] resourcePathArray = new string[]
            {
                "Assets/TeamMember/SeungHo.png",
                "Assets/TeamMember/JuYeop.png",
                "Assets/TeamMember/Hoon.png",
                "Assets/TeamMember/Jinu.png",
                "Assets/TeamMember/Min.png"
            };

            foreach(string imagePath in resourcePathArray)
            {
                Uri uri = GetResourceURI(null, imagePath);
                BitmapImage bitmapImage = new BitmapImage(uri);
                this.bitmapImageList.Add(bitmapImage);
            }

            this.currentImage.Source = this.bitmapImageList[this.currentIndex];
        }
        #endregion

        public string GetTitle()
        {
            return Title;
        }
    }
}
