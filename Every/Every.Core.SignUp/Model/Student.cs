using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.SignUp.Model
{
    public class Student : BindableBase, ICloneable
    {
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

        private string _pw;
        [JsonProperty("pw")]
        public string Pw
        {
            get => _pw;
            set
            {
                SetProperty(ref _pw, value);
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

        private string _birth_Year;
        [JsonProperty("birth_year")]
        public string Birth_Year
        {
            get => _birth_Year;
            set
            {
                SetProperty(ref _birth_Year, value);
            }
        }

        private string _school_Id;
        [JsonProperty("school_id")]
        public string School_Id
        {
            get => _school_Id;
            set
            {
                SetProperty(ref _school_Id, value);
            }
        }

        public object Clone()
        {
            return new Student
            {
                Email = this.Email,
                Pw = this.Pw,
                Name = this.Email,
                Phone = this.Phone,
                Birth_Year = this.Birth_Year,
                School_Id = this.School_Id
            };
        }
    }
}
