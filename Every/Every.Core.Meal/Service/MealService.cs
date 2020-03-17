using Every.Core.Meal.Service.Response;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Common;
using TNetwork.Data;

namespace Every.Core.Meal.Service
{
    public class MealService
    {
        private readonly string INQUIRY_MEAL_URL = "/meal";

        public async Task<TResponse<GetMealResponse>> GetMealMenus()
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(INQUIRY_MEAL_URL, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<GetMealResponse>>(response.Content);
            return resp;
        }
    }
}
