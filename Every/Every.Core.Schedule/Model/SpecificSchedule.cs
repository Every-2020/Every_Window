using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Schedule.Model
{
    public class SpecificSchedule : BindableBase, ICloneable
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

        private DateTime _start_Date;
        [JsonProperty("start_date")]
        public DateTime Start_Date
        {
            get => _start_Date;
            set
            {
                SetProperty(ref _start_Date, value);
            }
        }

        private DateTime _end_Date;
        [JsonProperty("end_date")]
        public DateTime End_Date
        {
            get => _end_Date;
            set
            {
                SetProperty(ref _end_Date, value);
            }
        }

        public object Clone()
        {
            return new SpecificSchedule
            {
                Idx = this.Idx,
                Title = this.Title,
                Content = this.Content,
                Start_Date = this.Start_Date,
                End_Date = this.End_Date
            };
        }
    }
}
