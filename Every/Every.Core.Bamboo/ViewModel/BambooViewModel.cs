﻿using Every.Core.Bamboo.Service;
using Every.Core.Member.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Every.Core.Bamboo.ViewModel
{
    public class BambooViewModel : BindableBase
    {
        BambooService bambooService = new BambooService();
        MemberService memberService = new MemberService();

        #region Properties
        // 게시물 작성 성공 여부
        public delegate void BambooPostResultReceivedHandler(object sender);
        public event BambooPostResultReceivedHandler BambooPostResultReceived;

        #region Commands
        public ICommand BambooPostCommand { get; set; }
        public ICommand BambooReplyCommand { get; set; }
        public ICommand BambooReplyDeleteCommand { get; set; }
        public ICommand BambooReplyModifyCommand { get; set; }
        #endregion

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

        // 특정 게시물 Idx 저장
        private int _specificIdx;
        public int SpecificIdx
        {
            get => _specificIdx;
            set => SetProperty(ref _specificIdx, value);
        }

        // Command 연속클릭 방지
        private bool _isEnable = true;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }

        // NavigationControl Modal Background
        public Visibility ModalBackGround { get; set; } = Visibility.Collapsed;

        // 요일 저장
        public string Day { get; set; }
        #endregion

        // 생성자
        public BambooViewModel()
        {
            BambooPostCommand = new DelegateCommand(OnBambooPost, CanBambooPost).ObservesProperty(() => BambooPostContent);
            // BambooReplyCommand = new DelegateCommand(OnBambooReply, CanBambooReply).ObservesProperty(() => BambooReplyContent);
        }

        private async void OnBambooPost()   
        {
            await BambooPost();
        }

        private bool CanBambooPost()
        {
            return (BambooPostContent != null) && (BambooPostContent != "") && (BambooPostContent != string.Empty);
        }

        //private async void OnBambooReply()
        //{
        //    await BambooReply();
        //}

        //private bool CanBambooReply()
        //{
        //    return (BambooReplyContent != null) && (BambooReplyContent != "") && (BambooReplyContent != string.Empty);
        //}


        // 전체 게시물 조회
        private async Task GetPosts()
        {
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

                        // 요일
                        GetDay(postsItems.Created_At);
                        postsItems.DayOfWeek = Day;

                        // 게시물 작성 시간
                        postsItems.PostWrittenTime = (DateTime.Now - postsItems.Created_At).Hours;

                        // 댓글 개수
                        var res = await bambooService.GetReplies(postsItems.Idx);
                        postsItems.ReplyCount = res.Data.Replies.Count;

                        PostsItems.Add((Model.Posts)postsItems.Clone());   
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("GetPosts Error : " + e.Message);
                }
            }
        }

        // 댓글 목록 조회
        public async Task GetReplies(int idx)
        {
            if(RepliesItems.Count > 0)
            {
                RepliesItems.Clear();
            }

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

                        repliesItems.ReplyWrittenTime = (DateTime.Now - repliesItems.Created_At).Hours;

                        var res = await memberService.GetStudentMemberInformation(item.Student_Idx);
                        repliesItems.WriterName = res.Data.MemberInformations.Name;

                        RepliesItems.Add((Model.Replies)repliesItems.Clone());
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("GetReplies Error : " + e.Message);
                }
            }
        }

        // 요일 구하기
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

        // 게시물 작성
        private async Task BambooPost()
        {
            IsEnable = false;

            if (BambooPostContent != null && BambooPostContent != "" && BambooPostContent != string.Empty)
            {
                var resp = await bambooService.MakePost(BambooPostContent);

                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BambooPostResultReceived?.Invoke(this);
                    BambooPostContent = string.Empty; // 작성한 댓글 내용 제거
                    await GetPosts();
                }
            }
            ModalBackGround = Visibility.Collapsed;

            IsEnable = true;
        }

        // 전체 게시물에서 댓글 작성, TODO : 게시물을 컨트롤로 따로 빼기, 그렇게 하면 컨트롤 내에서는 답글 텍스트 박스가 하나이므로 여러개를 처리할 필요가 없어짐.
        public async Task BambooReply(string replycontent, int? idx)
        {
            if (BambooReplyContent != null && BambooReplyContent != string.Empty && BambooReplyContent != "" &&  idx != null)
            {
                var resp = await bambooService.MakeReply(replycontent, (int)idx);

                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BambooReplyContent = string.Empty;
                    PostItems.Clear();
                    // TODO [수정] : 게시물 작성후 동기화할 때는 await 키워드가 있어도 잘 되는데, 왜 댓글 작성하고는 await을 붙이면 제대로 동기화가 안될까?
                    GetPosts();
                }
            }
            return;
        }

        // 특정 게시물 조회(댓글 개수 클릭시 보여주는 게시물)
        public async Task GetPost(int idx)
        {
            if(PostItems.Count > 0)
            {
                PostItems.Clear();
            }
            
            var resp = await bambooService.GetPost(idx);

            if(resp != null && resp.Data != null && resp.Status == 200)
            {
                try
                {
                    Model.Post postItems = new Model.Post();

                    postItems.Idx = resp.Data.Post.Idx;
                    SpecificIdx = resp.Data.Post.Idx; // 특정 게시물에서 댓글 작성을 위한 속성.

                    postItems.Content = resp.Data.Post.Content;
                    postItems.Created_At = resp.Data.Post.Created_At;

                    // 요일 구하기
                    GetDay(postItems.Created_At);
                    postItems.DayOfWeek = Day;

                    // 게시물 작성 시간
                    postItems.PostWrittenTime = (DateTime.Now - postItems.Created_At).Hours;

                    PostItems.Add((Model.Post)postItems.Clone());
                }
                catch(Exception e)
                {
                    Debug.WriteLine("GetPost Error : " + e.Message);
                }
            }

            return;
        }

        // 특정 게시물에서 댓글 작성
        public async Task BambooSpecificPostReply(string replycontent, int? idx)
        {
            if (BambooReplyContent != null && idx != null)
            {
                var resp = await bambooService.MakeReply(replycontent, (int)idx);

                if (resp.Status == (int)HttpStatusCode.Created)
                {
                    BambooReplyContent = string.Empty; // 특정 게시물에서 작성한 댓글내용 제거.
                    GetReplies((int)idx);                    
                    GetPosts();
                }
            }
            return;
        }

        // 특정 게시물에서 댓글 삭제
        public async Task BambooReplyDelete(int? replyIdx, int? postIdx)
        {
            if (replyIdx != null && postIdx != null)
            {
                var resp = await bambooService.DeleteReply((int)replyIdx);

                if (resp.Status == (int)HttpStatusCode.OK)
                {
                    RepliesItems.Clear(); // 특정 게시물 댓글내용 제거.
                    PostsItems.Clear(); // 전체 게시물 삭제.
                    await GetReplies((int)postIdx); // 특정 게시물 댓글목록 조회.
                    await GetPosts(); // 전체 게시물 조회.
                }
            }
            return;
        }

        // 특정 게시물에서 댓글 수정
        public async Task BambooReplyModify(int? replyIdx, string content, int? postIdx)
        {
            // TODO : 특정 게시물 댓글 수정 제작하기.
        }

        public async Task LoadDataAsync()
        {
            await GetPosts();
        }
    }
}
