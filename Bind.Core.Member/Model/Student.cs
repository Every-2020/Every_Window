using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class Student : Member
    {
        #region SERVERSYNCPROPERTY
        [JsonProperty("class_idx")]
        public int ClassIdx { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("parents_logs")]
        public List<ParentLog> ParentsLogs { get; set; }

        #endregion

        #region NOSERVERSYNCPROPERTY

        public ClassRoom Classroom { get; set; }

        #endregion

        public new object Clone()
        {
            Student student = new Student();

            student.Email = this.Email;
            student.Idx = this.Idx;
            student.Auth = this.Auth;
            student.Leave = this.Leave;
            student.Id = this.Id;
            student.Name = this.Name;
            student.Mobile = this.Mobile;
            student.ProfileImage = this.ProfileImage;
            student.Status = this.Status;
            student.StatusMessage = this.StatusMessage;
            student.LastConnected = this.LastConnected;
            student.LastUpdated = this.LastUpdated;
            student.JoinDate = this.JoinDate;
            student.IsMe = this.IsMe;
            student.ClassIdx = this.ClassIdx;
            student.Number = this.Number;
            student.ParentsLogs = this.ParentsLogs;
            student.Classroom = this.Classroom;

            return student;
        }
    }
}
