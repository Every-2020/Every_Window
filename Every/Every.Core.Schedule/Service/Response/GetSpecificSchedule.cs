using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Schedule.Service.Response
{
    public class GetSpecificSchedule : BindableBase
    {
        private List<Model.SpecificSchedule> _specificSchedule;
        public List<Model.SpecificSchedule> SpecificSchedule
        {
            get => _specificSchedule;
            set
            {
                SetProperty(ref _specificSchedule, value);
            }
        }
    }
}
