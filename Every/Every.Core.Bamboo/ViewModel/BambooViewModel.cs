using Every.Core.Bamboo.Service;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Bamboo.ViewModel
{
    public class BambooViewModel : BindableBase
    {
        BambooService bambooService = new BambooService();

        #region Properties
        // 게시글 목록 저장
        private ObservableCollection<Model.Post> _postsItems = new ObservableCollection<Model.Post>();
        public ObservableCollection<Model.Post> PostsItems
        {
            get => _postsItems;
            set
            {
                SetProperty(ref _postsItems, value);
            }
        }

        // 댓글 목록 저장
        private ObservableCollection<Model.Reply> _repliesItems = new ObservableCollection<Model.Reply>();
        public ObservableCollection<Model.Reply> RepliesItems
        {
            get => _repliesItems;
            set
            {
                SetProperty(ref _repliesItems, value);
            }
        }

        // 선택한 게시물
        private Model.Post _selectedPost = new Model.Post();
        public Model.Post SelectedPost
        {
            get => _selectedPost;
            set
            {
                SetProperty(ref _selectedPost, value);
            }
        }

        public string day { get; set; }
        #endregion
        private async Task GetPosts()
        {
            var resp = await bambooService.GetPosts();

            if(resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.Post postItems = new Model.Post();

                    foreach (var item in resp.Data.Posts)
                    {
                        postItems.Idx = item.Idx;
                        postItems.Title = item.Title;
                        postItems.Content = item.Content;
                        postItems.Created_At = item.Created_At;

                        GetDay(postItems.Created_At);
                        postItems.DayOfWeek = day;

                        var res = await bambooService.GetReplies(postItems.Idx);
                        postItems.ReplyCount = res.Data.Replies.Count;

                        PostsItems.Add((Model.Post)postItems.Clone());   
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        private async Task GetReplies()
        {
            var resp = await bambooService.GetReplies(SelectedPost.Idx);

            if(resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.Reply repliesItems = new Model.Reply();

                    foreach(var item in resp.Data.Replies)
                    {
                        repliesItems.Idx = item.Idx;
                        repliesItems.Content = item.Content;
                        repliesItems.Created_At = item.Created_At;
                        repliesItems.Student_Idx = item.Student_Idx;

                        RepliesItems.Add((Model.Reply)repliesItems.Clone());
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        private string GetDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    day = "월요일";
                    break;
                case DayOfWeek.Tuesday:
                    day = "화요일";
                    break;
                case DayOfWeek.Wednesday:
                    day = "수요일";
                    break;
                case DayOfWeek.Thursday:
                    day = "목요일";
                    break;
                case DayOfWeek.Friday:
                    day = "금요일";
                    break;
                case DayOfWeek.Saturday:
                    day = "토요일";
                    break;
                case DayOfWeek.Sunday:
                    day = "일요일";
                    break;
            }

            return day;
        }

        public async Task LoadDataAsync()
        {
            await GetPosts();
        }
    }
}
