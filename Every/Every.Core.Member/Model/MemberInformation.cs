using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Member.Model
{
    public class MemberInformation : BindableBase, ICloneable
    {
        private int _idx;
        [JsonProperty("idx")]
        public int Idx
        {
            get => _idx;
            set
            {
                SetProperty(ref _idx, value);
            }
        }

        private string _email;
        [JsonProperty("email")]
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
            }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private string _phone;
        [JsonProperty("phone")]
        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
            }
        }

        private int _birth_Year;
        [JsonProperty("birth_year")]
        public int Birth_Year
        {
            get => _birth_Year;
            set
            {
                SetProperty(ref _birth_Year, value);
            }
        }

        private int _school_Id;
        [JsonProperty("school_id")]
        public int School_Id
        {
            get => _school_Id;
            set
            {
                SetProperty(ref _school_Id, value);
            }
        }

        public object Clone()
        {
            return new MemberInformation
            {
                Idx = this.Idx,
                Email = this.Email,
                Name = this.Name,
                Phone = this.Phone,
                Birth_Year = this.Birth_Year,
                School_Id = this.School_Id
            };
        }
    }
}
