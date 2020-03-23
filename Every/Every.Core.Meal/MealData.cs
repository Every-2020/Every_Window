using Every.Core.Meal.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Meal
{
    public class MealData
    {
        public MealViewModel mealViewModel = new MealViewModel();
        
        public async Task LoadDataAsync()
        {
            await mealViewModel.LoadDataAsync();
        }
    }
}
