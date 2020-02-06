using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Every.Core.SignUp.Model
{
    public class Worker : BindableBase, ICloneable
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

        private int? _birth_Year;
        [JsonProperty("birth_year")]
        public int? Birth_Year
        {
            get => _birth_Year;
            set
            {
                SetProperty(ref _birth_Year, value);
            }
        }

        private string _work_Place;
        [JsonProperty("work_place")]
        public string Work_Place
        {
            get => _work_Place;
            set
            {
                SetProperty(ref _work_Place, value);
            }
        }

        private int? _work_Category;
        [JsonProperty("work_Category")]
        public int? Work_Category
        {
            get => _work_Category;
            set
            {
                SetProperty(ref _work_Category, value);
            }
        }

        public object Clone()
        {
            return new Worker
            {
                Email = this.Email,
                Pw = this.Pw,
                Name = this.Name,
                Phone = this.Phone, 
                Birth_Year = this.Birth_Year,
                Work_Place = this.Work_Place,
                Work_Category = this.Work_Category
            };
        }
    }
}
