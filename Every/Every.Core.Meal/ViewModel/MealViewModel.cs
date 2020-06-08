using Every.Core.Meal.Service;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Every.Core.Meal.ViewModel
{
    public class MealViewModel : BindableBase
    {
        MealService mealService = new MealService();

        #region Properties
        private string _breakfast;
        public string Breakfast
        {
            get => _breakfast;
            set
            {
                SetProperty(ref _breakfast, value);
            }
        }
        private string _lunch;
        public string Lunch
        {
            get => _lunch;
            set
            {
                SetProperty(ref _lunch, value);
            }
        }
        private string _dinner;
        public string Dinner
        {
            get => _dinner;
            set
            {
                SetProperty(ref _dinner, value);
            }
        }
        #endregion

        string before_process_meal; // 가공전 급식메뉴
        string temp_meal; // 임시 급식메뉴
        string complete_process_meal; // 가공후 급식메뉴

        private async Task GetMealMenus()
        {
            var resp = await mealService.GetMealMenus();

            if(resp.Status == (int)HttpStatusCode.NotFound)
            {
                Breakfast = "급식정보가 없습니다.";
                Lunch = "급식정보가 없습니다.";
                Dinner = "급식정보가 없습니다.";
            }
            else if(resp.Status == 200 && resp.Data != null && resp != null)
            {
                try
                {
                    foreach(var item in resp.Data.Meals)
                    {
                        if(item.Meal_Code >= 1 && item.Meal_Code <= 3)
                        {
                            MakeUpMealMenus(item.Meal_Name, item.Meal_Code);
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("GetmMealMenus Error : " + e.Message);
                }
            }
        }

        // 급식메뉴 가공
        private void MakeUpMealMenus(string meal_name, int meal_idx)
        {
            string menu = string.Empty;
            before_process_meal = meal_name;
            temp_meal = Regex.Replace(before_process_meal, @"\d", "");
            temp_meal = temp_meal.Replace("<br/>", " ");
            complete_process_meal = temp_meal + Environment.NewLine;
            complete_process_meal = complete_process_meal.Replace(".", "");

            string[] temp = complete_process_meal.Split(new string[] { " " }, StringSplitOptions.None); // 메뉴를 배열로 저장
            for (int i = 0; i < temp.Length; i++)
            {
                menu += temp[i] + Environment.NewLine;
            }
            
            if(meal_idx == 1)
            {
                Breakfast = menu;
            }
            if(meal_idx == 2)
            {
                Lunch =  menu;
            }
            if (meal_idx == 3)
            {
                Dinner = menu;
            }
        }

        public async Task LoadDataAsync()
        {
            await GetMealMenus();
        }
    }
}
