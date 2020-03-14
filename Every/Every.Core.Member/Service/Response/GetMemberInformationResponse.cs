using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Member.Service.Response
{
    public class GetMemberInformationResponse : BindableBase
    {
        private Model.MemberInformation _memberInformations;
        [JsonProperty("member")]
        public Model.MemberInformation MemberInformations
        {
            get => _memberInformations;
            set
            {
                SetProperty(ref _memberInformations, value);
            }
        }
    }
}
