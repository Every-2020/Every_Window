using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Meal
{
    public class MealData : BindableBase
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

        private DateTime _meal_Date;
        [JsonProperty("meal_date")]
        public DateTime Meal_Date
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

            }
        }
    }
}
