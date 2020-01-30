using BIND.Core.Member.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Service
{
    public class ClassRoomResponse
    {
        [JsonProperty("classes")]
        public List<ClassRoom> ClassRooms { get; set; }
    }
}
