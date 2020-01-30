using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class Receiver
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion
    }
}
