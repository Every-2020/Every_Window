using Every.Core.Member.Service.Response;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Common;
using TNetwork.Data;

namespace Every.Core.Member.Service
{
    public class MemberService
    {
        private readonly string STUDENT_MEMBER_INFORMATION_URL = "/member/student/"; // 학생 IDX로 회원 조회

        public async Task<TResponse<GetMemberInformationResponse>> GetStudentMemberInformation(int student_idx)
        {
            string requestUrl = STUDENT_MEMBER_INFORMATION_URL + student_idx;
            var client = new RestClient(Options.serverUrl);
            var restReqeust = new RestRequest(requestUrl, Method.GET);
            restReqeust.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restReqeust);

            var resp = JsonConvert.DeserializeObject<TResponse<GetMemberInformationResponse>>(response.Content);
            return resp;
        }
    }
}
