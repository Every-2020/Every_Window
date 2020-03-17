using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Schedule.Service.Response
{
    public class GetAllSchedules : BindableBase
    {
        private List<Model.AllSchedules> _allSchedules;
        [JsonProperty("schedules")]
        public List<Model.AllSchedules> AllSchedules
        {
            get => _allSchedules;
            set
            {
                SetProperty(ref _allSchedules, value);
            }
        }
    }
}
