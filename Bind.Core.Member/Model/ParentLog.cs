using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class ParentLog
    {
        #region SERVERSYNCPROPERTY
        [JsonProperty("parent_idx")]
        public int ParentIdx { get; set; }
        #endregion

        #region NOSERVERSYNCPROPERTY
        public string Name { get; set; }

        public string PhoneNum { get; set; }

        public string Image { get; set; }
        #endregion
    }
}
