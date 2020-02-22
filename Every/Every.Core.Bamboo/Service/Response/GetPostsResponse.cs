using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Service.Response
{
    public class GetPostsResponse : BindableBase
    {
        private List<Model.Post> _posts;
        [JsonProperty("posts")]
        public List<Model.Post> Posts
        {
            get => _posts;
            set
            {
                SetProperty(ref _posts, value);
            }
        }
    }
}
