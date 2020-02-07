using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.SignUp.Model
{
    public class School : BindableBase, ICloneable
    {
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

        private string _office_Id;
        [JsonProperty("office_id")]
        public string Office_Id
        {
            get => _office_Id;
            set
            {
                SetProperty(ref _office_Id, value);
            }
        }

        private string _school_Name;
        [JsonProperty("school_name")]
        public string School_Name
        {
            get => _school_Name;
            set
            {
                SetProperty(ref _school_Name, value);
            }
        }

        private string _school_Location;
        [JsonProperty("school_location")]
        public string School_Location
        {
            get => _school_Location;
            set
            {
                SetProperty(ref _school_Location, value);
            }
        }

        public object Clone()
        {
            return new School
            {
                School_Id = this.School_Id,
                Office_Id = this.Office_Id,
                School_Name = this.School_Name,
                School_Location = this.School_Location
            };
        }
    }
}
