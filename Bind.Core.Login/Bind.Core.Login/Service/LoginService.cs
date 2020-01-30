
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TNetwork;
using TNetwork.Common;
using TNetwork.Data;

namespace BIND.Core.Login.Network
{
    public class LoginService
    {
        private NetworkManager networkManager = new NetworkManager();

        public string FILE_URL = "/upload/{0}";
        private readonly Encoding encoding = Encoding.UTF8;


        private const string LOGIN_URL = "auth/login";
        private const string MEMBER_URL = "members/";
        private const string LOGOUT_URL = MEMBER_URL + "logout";
        private const string TOKEN_REFRESH_URL = "/token/refresh";
        public async Task<TResponse<TokenInfo>> Login(string id, string pw)
        {

            var jObj = new JObject();
            jObj["device"] = "PC";
            jObj["id"] = id;
            jObj["pw"] = Sha512Hash(pw);
            var resp = await networkManager.GetResponse<TokenInfo>(Options.loginUrl, Method.POST, jObj.ToString());

            if (resp.Data != null)
            {
                Options.tokenInfo.Token = resp.Data.Token;
                Options.tokenInfo.RefreshToken = resp.Data.RefreshToken;
            }
            //resp.Data.Member.IsMe = true;
            return resp;
        }

        private string Sha512Hash(string str)
        {
            var sha512 = new SHA512CryptoServiceProvider();
            byte[] resultHash = sha512.ComputeHash(Encoding.Default.GetBytes(str));
            string transPwd = string.Empty;

            foreach (byte x in resultHash)
            {
                transPwd += $"{x:x2}";
            }

            return transPwd;
        }

        public async Task Logout()
        {
            var jObj = new JObject();
            jObj["device"] = "PC";

            var resp = await networkManager.GetResponse<TokenInfo>(Options.logoutUrl, Method.POST, jObj.ToString());
        }

        public void SettingHttpRequest(string serverUrl)
        {
            networkManager.SetHTTPRequestURL(serverUrl, LOGIN_URL, LOGOUT_URL, TOKEN_REFRESH_URL);
        }

        //public async Task<TResponse<Nothing>> UpdateMember(Member.Model.Member member)
        //{
        //    JObject jObj = new JObject();

        //    var split = member.ProfileImage.Split('/');

        //    jObj["mobile"] = member.Mobile;
        //    jObj["email"] = member.Email;
        //    jObj["profile_image"] = split[split.Length - 1];
        //    jObj["status_message"] = member.StatusMessage;

        //    var resp = await networkManager.GetResponse<Nothing>(MEMBER_URL + member.Id, Method.PUT, jObj.ToString());

        //    return resp;
        //}

        //private string GetFormattedFileUrl(string extension)
        //{
        //    return string.Format(FILE_URL, extension);
        //}

        //public async Task<IRestResponse> UploadFile(Dictionary<string, object> postParameters, string extension)
        //{
        //    string formDataBoundary = String.Format("---------{0:N}", Guid.NewGuid());
        //    string contentType = "multipart/form-data; boundary=" + formDataBoundary;

        //    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

        //    return await networkManager.PostFormAsync(GetFormattedFileUrl(extension), contentType, formData, extension);
        //}

        //private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        //{
        //    Stream formDataStream = new MemoryStream();

        //    foreach (var param in postParameters)
        //    {
        //        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

        //        if (param.Value is MultiPartFile)
        //        {
        //            MultiPartFile fileToUpload = (MultiPartFile)param.Value;

        //            string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
        //                boundary,
        //                param.Key,
        //                fileToUpload.FileName ?? param.Key,
        //                fileToUpload.ContentType ?? "application/octet-stream");

        //            formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

        //            formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);

        //            formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

        //            string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
        //                boundary,
        //                "name",
        //                fileToUpload.Guid);
        //            formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
        //        }
        //    }

        //    string footer = "\r\n--" + boundary + "--\r\n";
        //    formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

        //    formDataStream.Position = 0;
        //    byte[] formData = new byte[formDataStream.Length];
        //    formDataStream.Read(formData, 0, formData.Length);

        //    formDataStream.Close();

        //    return formData;
        //}
    }
}
