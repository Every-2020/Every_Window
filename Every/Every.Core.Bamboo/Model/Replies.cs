using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Model
{
    public class Replies : BindableBase, ICloneable
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

        private int _student_Idx;
        [JsonProperty("student_idx")]
        public int Student_Idx
        {
            get => _student_Idx;
            set
            {
                SetProperty(ref _student_Idx, value);
            }
        }

        public object Clone()
        {
            return new Replies
            {
                Idx = this.Idx,
                Content = this.Content,
                Created_At = this.Created_At,
                Student_Idx = this.Student_Idx
            };
        }
    }
}
