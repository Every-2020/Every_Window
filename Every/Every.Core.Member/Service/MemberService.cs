using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Member.Service
{
    public class MemberService : BindableBase
    {
        private List<Model.MemberInformation> _memberInformations;
        [JsonProperty("member")]
        public List<Model.MemberInformation> MemberInformations
        {
            get => _memberInformations;
            set
            {
                SetProperty(ref _memberInformations, value);
            }
        }
    }
}
