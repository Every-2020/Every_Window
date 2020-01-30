using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public enum AuthType
    {
        ADMIN,
        STUDENT,
        TEACHER,
        PARENT,
        NONE
    }

    public class Member : BindableBase, ICloneable
    {
        #region SERVERSYNCPROPERTY
        [JsonProperty("idx")]
        public int Idx { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        private string _mobile;
        [JsonProperty("mobile")]
        public string Mobile {
            get => _mobile;
            set
            {
                SetProperty(ref _mobile, value);
            }
        }

        private string _email;
        [JsonProperty("email")]
        public string Email {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
            }
        }

        private string _profileImage;
        [JsonProperty("profile_image")]
        public string ProfileImage {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private string _statusMessage;

        [JsonProperty("status_message")]
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                SetProperty(ref _statusMessage, value);
            }
        }

        [JsonProperty("auth")]
        public AuthType Auth { get; set; } = AuthType.NONE;

        [JsonProperty("leave")]
        public int Leave { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("join_date")]
        public DateTime JoinDate { get; set; }

        [JsonProperty("last_connected")]
        public DateTime LastConnected { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        #endregion

        #region NOSERVERSYNCPROPERTY

        [JsonIgnore]
        public bool IsMe { get; set; }

        #endregion

        public object Clone()
        {
            Member member = new Member();

            member.Email = this.Email;
            member.Idx = this.Idx;
            member.Auth = this.Auth;
            member.Leave = this.Leave;
            member.Id = this.Id;
            member.Name = this.Name;
            member.Mobile = this.Mobile;
            member.ProfileImage = this.ProfileImage;
            member.Status = this.Status;
            member.StatusMessage = this.StatusMessage;
            member.LastConnected = this.LastConnected;
            member.LastUpdated = this.LastUpdated;
            member.JoinDate = this.JoinDate;
            member.IsMe = this.IsMe;

            return member;
        }
    }
}
