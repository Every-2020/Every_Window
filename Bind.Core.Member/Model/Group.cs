using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class DepartmentLog
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("department_idx")]
        public int DepartmentIdx { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("class_idx")]
        public int? ClassIdx { get; set; }
        
        #endregion

        #region NOSERVERSYNCPROPERTY

        public string DepartmentName { get; set; }

        #endregion
    }

    public class Department
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("idx")]
        public int Idx { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
       
        #endregion

    }

    public class ClassRoom
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("idx")]
        public int Idx { get; set; }

        [JsonProperty("grade")]
        public int Grade { get; set; }

        [JsonProperty("room")]
        public int Room { get; set; }

        public override string ToString() => $"{this.Grade}학년 {this.Room}반";

        #endregion
    }
}
