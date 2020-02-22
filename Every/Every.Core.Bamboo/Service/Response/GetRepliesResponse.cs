using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Service.Response
{
    public class GetRepliesResponse : BindableBase
    {
        private List<Model.Reply> _replies;
        [JsonProperty("replies")]
        public List<Model.Reply> Replies
        {
            get => _replies;
            set
            {
                SetProperty(ref _replies, value);
            }
        }
    }
}
