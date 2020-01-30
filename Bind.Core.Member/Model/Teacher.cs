using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member.Model
{
    public class Teacher : Member
    {
        #region NOSERVERSYNCPROPERTY

        public string PrimaryDepartment { get; set; }
        
        public string MainPosition { get; set; }

        #endregion

        public new object Clone()
        {
            Teacher teacher = new Teacher();

            teacher.Email = this.Email;
            teacher.Idx = this.Idx;
            teacher.Auth = this.Auth;
            teacher.Leave = this.Leave;
            teacher.Id = this.Id;
            teacher.Name = this.Name;
            teacher.Mobile = this.Mobile;
            teacher.ProfileImage = this.ProfileImage;
            teacher.Status = this.Status;
            teacher.StatusMessage = this.StatusMessage;
            teacher.LastConnected = this.LastConnected;
            teacher.LastUpdated = this.LastUpdated;
            teacher.JoinDate = this.JoinDate;
            teacher.IsMe = this.IsMe;
            teacher.PrimaryDepartment = this.PrimaryDepartment;
            teacher.MainPosition = this.MainPosition;

            return teacher;
        }
    }
}
