using Every.Core.SignUp.Service.Response;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TNetwork;
using TNetwork.Data;

namespace Every.Core.SignUp.Service
{
    public class SignUpService
    {
        #region SettingHttpRequest
        private const string LOGIN_URL = "/auth/login";
        private const string MEMBER_URL = "members/";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";
        #endregion

        public readonly string SIGNUP_STUDENT_URL = "/auth/register/student"; // 학생용 회원가입
        public readonly string SIGNUP_WORKER_URL = "/auth/register/worker"; // 직장인용 회원가입

        public readonly string SEARCH_SCHOOL_URL = "/school?query="; // 학교 목록 조회

        public NetworkManager networkManager = new NetworkManager();

        /// <summary>
        /// 학생 회원가입 메소드
        /// </summary>
        /// <param name="email", 아이디(이메일 사용)></param>
        /// <param name="pw", 비밀번호></param>
        /// <param name="name", 이름></param>
        /// <param name="phone", 전화번호("-" 입력해줘야 함.)></param>
        /// <param name="birth_year", 태어난 년도></param>
        /// <param name="school_id", 학교 정보></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> Student_SignUp(string email, string pw, string name, string phone, int? birth_year, string school_id)
        {
            JObject jObject = new JObject();
            jObject["email"] = email;
            jObject["pw"] = pw;
            jObject["name"] = name;
            jObject["phone"] = phone;
            jObject["birth_year"] = birth_year;
            jObject["school_id"] = school_id;
            return await networkManager.GetResponse<Nothing>(SIGNUP_STUDENT_URL, Method.POST, jObject.ToString());
        }

        /// <summary>
        /// 직장인용 회원가입 메소드
        /// </summary>
        /// <param name="email", 아이디(이메일 사용)></param>
        /// <param name="pw", 비밀번호></param>
        /// <param name="name", 이름></param>
        /// <param name="phone", 전화번호("-" 입력해줘야 함.)></param>
        /// <param name="birth_year", 태어난 년도></param>
        /// <param name="work_place", 근무 직장></param>
        /// <param name="work_category", 근무 직종></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> Worker_SignUp(string email, string pw, string name, string phone, int? birth_year, string work_place, int? work_category)
        {
            JObject jObject = new JObject();
            jObject["email"] = email;
            jObject["pw"] = pw;
            jObject["name"] = name;
            jObject["phone"] = phone;
            jObject["birth_year"] = birth_year;
            jObject["work_place"] = work_place;
            jObject["work_category"] = work_category;
            return await networkManager.GetResponse<Nothing>(SIGNUP_WORKER_URL, Method.POST, jObject.ToString());
        }

        /// <summary>
        /// 학교 목록 조회 메소드
        /// </summary>
        /// <param name="schoolName", 매개변수(Query Parameters)></param>
        /// <returns></returns>
        public async Task<TResponse<GetSchoolListResponse>> GetSchoolList(string schoolName)
        {
            string requestUrl = SEARCH_SCHOOL_URL + schoolName;
            return await networkManager.GetResponse<GetSchoolListResponse>(requestUrl, Method.GET, null);
        }

        public void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
        }
    }
}
