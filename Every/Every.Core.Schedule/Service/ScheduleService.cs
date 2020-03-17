using Every.Core.Schedule.Service.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Common;
using TNetwork.Data;

namespace Every.Core.Schedule.Service
{
    public class ScheduleService 
    {
        public readonly string INQUIRY_SCHEDULE_URL = "/schedule"; // 모든 일정 조회
        public readonly string INQUIRY_SPECIFIC_SCHEDULE_URL = "/schedule/"; // 특정 일정 조회
        public readonly string CREATE_SCHEDULE_URL = "/schedule"; // 일정 생성
        public readonly string MODIFY_SCHEDULE_URL = "/schedule/"; // 일정 수정 
        public readonly string DELETE_SCHEDULE_URL = "/schedule/"; // 일정 삭제 

        /// <summary>
        /// 전체 일정 조회 메소드
        /// </summary>
        /// <returns></returns>
        public async Task<TResponse<GetAllSchedules>> GetAllSchedules()
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(INQUIRY_SCHEDULE_URL, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<GetAllSchedules>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 특정 일정 조회 메소드
        /// </summary>
        /// <param name="idx", 일정 고유 idx></param>
        /// <returns></returns>
        public async Task<TResponse<GetSpecificSchedule>> GetSpecificSchedule(int idx)
        {
            string requestUrl = INQUIRY_SPECIFIC_SCHEDULE_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<GetSpecificSchedule>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 일정 생성 메소드
        /// </summary>
        /// <param name="title", 일정 제목></param>
        /// <param name="content", 일정 내용></param>
        /// <param name="start_date", 시작 날짜></param>
        /// <param name="end_date", 종료 날짜></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> CreateSchedule(string title, string content, DateTime start_date, DateTime end_date)
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(CREATE_SCHEDULE_URL, Method.POST);
            JObject jObject = new JObject();
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["start_date"] = start_date;
            jObject["end_date"] = end_date;
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jObject.ToString(), ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 일정 수정 메소드
        /// </summary>
        /// <param name="idx", 수정할 일정 idx></param>
        /// <param name="title", 수정한 일정 제목></param>
        /// <param name="content", 수정한 일정 내용></param>
        /// <param name="start_date", 수정한 일정 시작 날짜></param>
        /// <param name="end_date", 수정한 일정 종료 날짜></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> ModifySchedule(int idx, string title, string content, DateTime start_date, DateTime end_date)
        {
            string requestUrl = MODIFY_SCHEDULE_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.PUT);
            JObject jObject = new JObject();
            jObject["title"] = title;
            jObject["content"] = content;
            jObject["start_date"] = start_date;
            jObject["end_date"] = end_date;
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jObject.ToString(), ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 일정 삭제 메소드
        /// </summary>
        /// <param name="idx", 삭제할 일정 idx></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> DeleteSchedule(int idx)
        {
            string requestUrl = DELETE_SCHEDULE_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restReqeust = new RestRequest(requestUrl, Method.DELETE);
            restReqeust.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restReqeust);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }
    }
}
