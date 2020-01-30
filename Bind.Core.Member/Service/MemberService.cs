using BIND.Core.Member.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork;
using TNetwork.Data;

namespace BIND.Core.Member.Service
{
    internal class MemberService
    {
        public NetworkManager networkManager = new NetworkManager();

        private const string LOGIN_URL = "auth/login";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";


        private const string MEMBER_URL = "members/";
        public const string CLASS_URL = "class/";

        internal void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
        }

        internal async Task<TResponse<MemberResponse>> GetUsers()
        {
            return await networkManager.GetResponse<MemberResponse>(MEMBER_URL, Method.GET, null);
        }


        public async Task<TResponse<ClassRoomResponse>> GetClasses()
        {
            return await networkManager.GetResponse<ClassRoomResponse>(CLASS_URL, Method.GET, null);
        }

        public async Task<IRestResponse> UploadFile(Dictionary<string, object> dicPost, string extension)
        {
            return await networkManager.UploadFile(dicPost, extension);
        }

        public async Task<TResponse<Nothing>> UpdateMember(Member.Model.Member member)
        {
            JObject jObj = new JObject();

            var split = member.ProfileImage.Split('/');

            jObj["mobile"] = member.Mobile;
            jObj["email"] = member.Email;
            jObj["profile_image"] = split[split.Length - 1];
            jObj["status_message"] = member.StatusMessage;

            var resp = await networkManager.GetResponse<Nothing>(MEMBER_URL + member.Id, Method.PUT, jObj.ToString());

            return resp;
        }
    }
}
