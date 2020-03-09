using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Service.Response
{
    public class GetPostResponse : BindableBase
    {
        private List<Model.Post> _post;
        [JsonProperty("post")]
        public List<Model.Post> Post
        {
            get => _post;
            set
            {
                SetProperty(ref _post, value);
            }
        }
    }
}
