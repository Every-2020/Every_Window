using Every.Core.Bamboo.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Every.Core.Bamboo.ViewModel
{
    public class BambooViewModel : BindableBase
    {
        BambooService bambooService = new BambooService();

        #region Properties
        public delegate void BambooPostResultReceivedHandler(object sender);
        public event BambooPostResultReceivedHandler BambooPostResultReceived;

        // 게시글 목록 저장
        private ObservableCollection<Model.Posts> _postsItems = new ObservableCollection<Model.Posts>();
        public ObservableCollection<Model.Posts> PostsItems
        {
            get => _postsItems;
            set
            {
                SetProperty(ref _postsItems, value);
            }
        }

        // 댓글 목록 저장
        private ObservableCollection<Model.Replies> _repliesItems = new ObservableCollection<Model.Replies>();
        public ObservableCollection<Model.Replies> RepliesItems
        {
            get => _repliesItems;
            set
            {
                SetProperty(ref _repliesItems, value);
            }
        }

        // 게시글 저장
        private ObservableCollection<Model.Post> _postItems = new ObservableCollection<Model.Post>();
        public ObservableCollection<Model.Post> PostItems
        {
            get => _postItems;
            set
            {
                SetProperty(ref _postItems, value);
            }
        }

        // 댓글 저장
        private ObservableCollection<Model.Post> _replyItems = new ObservableCollection<Model.Post>();
        public ObservableCollection<Model.Post> ReplyItems
        {
            get => _replyItems;
            set
            {
                SetProperty(ref _replyItems, value);
            }
        }

        // 선택한 게시물
        private Model.Posts _selectedPost = new Model.Posts();
        public Model.Posts SelectedPost
        {
            get => _selectedPost;
            set
            {
                SetProperty(ref _selectedPost, value);
            }
        }

        // 게시글 작성 내용
        private string _bambooPostContent;
        public string BambooPostContent
        {
            get => _bambooPostContent;
            set
            {
                if(value.Length <= 500)
                {
                    SetProperty(ref _bambooPostContent, value);
                }
                return;
            }
        }

        // 댓글 작성 내용
        private string _bambooReplyContent;
        public string BambooReplyContent
        {
            get => _bambooReplyContent;
            set
            {
                if(value.Length <= 250)
                { 
                    SetProperty(ref _bambooReplyContent, value);
                }
                return;
            }
        }

        public string Day { get; set; }

        public ICommand BambooPostCommand { get; set; }
        public ICommand BambooReplyCommand { get; set; }
        #endregion

        public BambooViewModel()
        {
            BambooPostCommand = new DelegateCommand(OnBambooPost, CanBambooPost).ObservesProperty(() => BambooPostContent);
            BambooReplyCommand = new DelegateCommand(OnBambooReply, CanBambooReply).ObservesProperty(() => BambooReplyContent);
        }

        private async void OnBambooPost()   
        {
            await BambooPost();
        }

        private bool CanBambooPost()
        {
            return (BambooPostContent != null) && (BambooPostContent != "") && (BambooPostContent != string.Empty);
        }

        private async void OnBambooReply()
        {
            await BambooReply();
        }

        private bool CanBambooReply()
        {
            return (BambooReplyContent != null) && (BambooReplyContent != "") && (BambooReplyContent != string.Empty);
        }


        private async Task GetPosts()
        {
            if(PostsItems != null)
            {
                PostsItems.Clear();
            }

            var resp = await bambooService.GetPosts();

            if(resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.Posts postsItems = new Model.Posts();

                    foreach (var item in resp.Data.Posts)
                    {
                        postsItems.Idx = item.Idx;
                        postsItems.Title = item.Title;
                        postsItems.Content = item.Content;
                        postsItems.Created_At = item.Created_At;

                        GetDay(postsItems.Created_At);
                        postsItems.DayOfWeek = Day;

                        postsItems.PostWrittenTime = (DateTime.Now - postsItems.Created_At).Hours;

                        var res = await bambooService.GetReplies(postsItems.Idx);
                        postsItems.ReplyCount = res.Data.Replies.Count;

                        PostsItems.Add((Model.Posts)postsItems.Clone());   
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        public async Task GetReplies(int idx)
        {
            if(RepliesItems != null)
            {
                RepliesItems.Clear();
            }

            if(SelectedPost != null)
            {
                var resp = await bambooService.GetReplies(idx);

                if(resp != null && resp.Status == 200 && resp.Data != null)
                {
                    try
                    {
                        Model.Replies repliesItems = new Model.Replies();

                        foreach(var item in resp.Data.Replies)
                        {
                            repliesItems.Idx = item.Idx;
                            repliesItems.Content = item.Content;
                            repliesItems.Created_At = item.Created_At;
                            repliesItems.Student_Idx = item.Student_Idx;

                            RepliesItems.Add((Model.Replies)repliesItems.Clone());
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
        }

        private string GetDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    Day = "월요일";
                    break;
                case DayOfWeek.Tuesday:
                    Day = "화요일";
                    break;
                case DayOfWeek.Wednesday:
                    Day = "수요일";
                    break;
                case DayOfWeek.Thursday:
                    Day = "목요일";
                    break;
                case DayOfWeek.Friday:
                    Day = "금요일";
                    break;
                case DayOfWeek.Saturday:
                    Day = "토요일";
                    break;
                case DayOfWeek.Sunday:
                    Day = "일요일";
                    break;
            }

            return Day;
        }

        private async Task BambooPost()
        {
            if(BambooPostContent != null)
            {
                var resp = await bambooService.MakePost(BambooPostContent);

                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BambooPostResultReceived?.Invoke(this);
                    BambooPostContent = string.Empty;
                    PostsItems.Clear();
                    await GetPosts();
                }
            }
            return;
        }

        private async Task BambooReply()
        {
            if (BambooReplyContent != null && SelectedPost != null)
            {
                var resp = await bambooService.MakeReply(BambooReplyContent, SelectedPost.Idx);

                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BambooReplyContent = string.Empty;
                    PostsItems.Clear();
                    await GetPosts();
                }
            }
            return;
        }

        public async Task GetPost(int idx)
        {
            if(PostItems != null)
            {
                PostItems.Clear();
            }

            if(SelectedPost != null)
            {
                var resp = await bambooService.GetPost(idx);

                if(resp != null && resp.Data != null && resp.Status == 200)
                {
                    try
                    {
                        Model.Post postItems = new Model.Post();
                        
                        postItems.Idx = resp.Data.Post.Idx;
                        postItems.Content = resp.Data.Post.Content;
                        postItems.Created_At = resp.Data.Post.Created_At;

                        PostItems.Add((Model.Post)postItems.Clone());
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
            return;
        }

        public async Task LoadDataAsync()
        {
            await GetPosts();
        }
    }
}
