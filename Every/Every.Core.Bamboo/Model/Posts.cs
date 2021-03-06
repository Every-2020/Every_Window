﻿using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.Bamboo.Model
{
    public class Posts : BindableBase, ICloneable
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

        private string _title;
        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
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

        private string _dayOfWeek;
        [JsonIgnore]
        public string DayOfWeek
        {
            get => _dayOfWeek;
            set
            {
                SetProperty(ref _dayOfWeek, value);
            }
        }

        private int _postWrittenTime;
        [JsonIgnore]
        public int PostWrittenTime
        {
            get => _postWrittenTime;
            set
            {
                SetProperty(ref _postWrittenTime, value);
            }
        }

        private int _replyCount;
        [JsonIgnore]
        public int ReplyCount
        {
            get => _replyCount;
            set
            {
                SetProperty(ref _replyCount, value);
            }
        }

        public object Clone()
        {
            return new Posts
            {
                Idx = this.Idx,
                Title = this.Title,
                Content = this.Content,
                Created_At = this.Created_At,
                DayOfWeek = this.DayOfWeek,
                PostWrittenTime = this.PostWrittenTime,
                ReplyCount = this.ReplyCount
            };
        }
    }
}
