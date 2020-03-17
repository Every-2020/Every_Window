using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Meal.Service.Response
{
    public class GetMealResponse : BindableBase
    {
        private List<Model.Meal> _meals;
        [JsonProperty("meals")]
        public List<Model.Meal> Meals
        {
            get => _meals;
            set
            {
                SetProperty(ref _meals, value);
            }
        }
    }
}
