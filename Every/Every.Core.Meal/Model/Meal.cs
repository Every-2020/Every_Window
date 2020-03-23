using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Meal.Model
{
    public class Meal : BindableBase, ICloneable
    {
        private int _meal_Code;
        [JsonProperty("meal_code")]
        public int Meal_Code
        {
            get => _meal_Code;
            set
            {
                SetProperty(ref _meal_Code, value);
            }
        }

        private string _meal_Date;
        [JsonProperty("meal_date")]
        public string Meal_Date
        {
            get => _meal_Date;
            set
            {
                SetProperty(ref _meal_Date, value);
            }
        }

        private string _meal_name;
        [JsonProperty("meal_name")]
        public string Meal_Name
        {
            get => _meal_name;
            set
            {
                SetProperty(ref _meal_name, value);
            }
        }

        public object Clone()
        {
            return new Meal
            {
                Meal_Code = this.Meal_Code,
                Meal_Date = this.Meal_Date,
                Meal_Name = this.Meal_Name
            };
        }
    }
}
