using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class Parent : Member
    {
        #region SERVERSYNCPROPERTY

        [JsonProperty("children_logs")]
        public List<ChildrenLog> childrenLogs { get; set; }

        #endregion

        public new object Clone()
        {
            Parent parent = new Parent();

            parent.Email = this.Email;
            parent.Idx = this.Idx;
            parent.Auth = this.Auth;
            parent.Leave = this.Leave;
            parent.Id = this.Id;
            parent.Name = this.Name;
            parent.Mobile = this.Mobile;
            parent.ProfileImage = this.ProfileImage;
            parent.Status = this.Status;
            parent.StatusMessage = this.StatusMessage;
            parent.LastConnected = this.LastConnected;
            parent.LastUpdated = this.LastUpdated;
            parent.JoinDate = this.JoinDate;
            parent.IsMe = this.IsMe;
            parent.childrenLogs = this.childrenLogs;

            return parent;
        }
    }
}
