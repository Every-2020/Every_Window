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
        private ObservableCollection<Model.Post> _postsItems = new ObservableCollection<Model.Post>();
        public ObservableCollection<Model.Post> PostsItems
        {
            get => _postsItems;
            set
            {
                SetProperty(ref _postsItems, value);
            }
        }
        #endregion

        private async Task GetPosts()
        {
            var resp = await bambooService.GetPosts();

            if(resp != null && resp.Status == 200 && resp.Data != null)
            {
                try
                {
                    Model.Post postItems = new Model.Post();

                    foreach(var item in resp.Data.Posts)
                    {
                        postItems.Idx = item.Idx;
                        postItems.Title = item.Title;
                        postItems.Content = item.Content;
                        postItems.Created_At = item.Created_At;

                        PostsItems.Add((Model.Post)item.Clone());
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        public async Task LoadDataAsync()
        {
            await GetPosts();
        }
    }
}
