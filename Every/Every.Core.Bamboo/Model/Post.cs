using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Model
{
    public class Post : BindableBase, ICloneable
    {
        private int _idx;
        [JsonProperty("idx")]
        public int Idx
        {
            get => _idx;
            set
            {
                SetProperty(ref _idx, value);
            }
        }

        private string _content;
        [JsonProperty("content")]
        public string Content
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value);
            }
        }

        private DateTime _created_At;
        [JsonProperty("created_at")]
        public DateTime Created_At
        {
            get => _created_At;
            set
            {
                SetProperty(ref _created_At, value);
            }
        }

        public object Clone()
        {
            return new Post
            {
                Idx = this.Idx,
                Content = this.Content,
                Created_At = this.Created_At
            };
        }
    }
}
