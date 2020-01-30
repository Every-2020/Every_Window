using BIND.Core.Member.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Service
{
    public class MemberResponse
    {
        [JsonProperty("students")]
        public List<Student> Students = new List<Student>();

        [JsonProperty("teachers")]
        public List<Teacher> Teachers = new List<Teacher>();

        [JsonProperty("parents")]
        public List<Parent> Parents = new List<Parent>();
    }
}
