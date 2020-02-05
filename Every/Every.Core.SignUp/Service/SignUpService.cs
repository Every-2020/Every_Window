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
        private const string LOGIN_URL = "/auth/login";
        private const string MEMBER_URL = "members/";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";

        public readonly string SIGNUP_URL = "/auth/register/student"; // 학생용 회원가입

        public NetworkManager networkManager = new NetworkManager();

        /// <summary>
        /// 회원가입 메소드
        /// </summary>
        /// <param name="email", 아이디(이메일 사용)></param>
        /// <param name="pw", 비밀 번호></param>
        /// <param name="name", 이름></param>
        /// <param name="phone", 전화번호("-" 입력해줘야 함.)></param>
        /// <param name="birth_year", 태어난 년도></param>
        /// <param name="school_id", 학교 정보></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> Student_SignUp(string email, string pw, string name, string phone, string birth_year, string school_id)
        {
            JObject jObject = new JObject();
            jObject["email"] = email;
            jObject["pw"] = pw;
            jObject["name"] = name;
            jObject["phone"] = phone;
            jObject["birth_year"] = birth_year;
            jObject["school_id"] = school_id;
            return await networkManager.GetResponse<Nothing>(SIGNUP_URL, Method.POST, jObject.ToString());
        }

        public void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
        }
    }
}
