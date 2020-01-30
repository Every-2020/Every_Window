using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class ChildrenLog
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("child_idx")]
        public int ChildIdx { get; set; }

        #endregion
    }
}
