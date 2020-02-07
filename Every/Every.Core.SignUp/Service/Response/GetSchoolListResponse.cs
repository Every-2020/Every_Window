using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.SignUp.Service.Response
{
    public class GetSchoolListResponse : BindableBase
    {
        private List<Model.School> _schools;
        [JsonProperty("schools")]
        public List<Model.School> Schools
        {
            get => _schools;
            set
            {
                SetProperty(ref _schools, value);
            }
        }
    }
}
