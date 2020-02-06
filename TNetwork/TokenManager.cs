using TNetwork.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Common;

namespace TNetwork
{
    public partial class NetworkManager : IServerProtocol
    {
        public const int TOKEN_EXPIRED = 410;

        public async Task TokenRefresh()
        {
            JObject jObj = new JObject();
            jObj["refresh_token"] = Options.tokenInfo.RefreshToken;
            var resp = await GetResponse<string>(Options.tokenRefreshUrl, Method.POST, jObj.ToString());
            if (resp.Status == (int)System.Net.HttpStatusCode.OK)
            {
                Options.tokenInfo.Token = JObject.Parse(resp.Data)?["data"]?["new_token"]?.ToString();
            }        
            else
            {
                if (resp.Status == TOKEN_EXPIRED)
                {
                    if(Options.password != null)
                    {
                        jObj = new JObject();
                        jObj["device"] = "PC";
                        jObj["id"] = Options.id;
                        jObj["pw"] = LoginInfo.Sha512Hash(Options.password);
                        var response = await GetResponse<TokenInfo>(Options.loginUrl, Method.POST, jObj.ToString());

                        Options.tokenInfo = new TokenInfo()
                        {
                            RefreshToken = response.Data.RefreshToken,
                            Token = response.Data.Token
                        };
                    }
                }
            }
        }

        public Task GetResponse<T>(string cOUNSEL_URL, object gET, object p)
        {
            throw new NotImplementedException();
        }

        public bool CheckTokenExpired(IRestResponse response)
        {
            if ((int)response.ResponseStatus == TOKEN_EXPIRED)
            {
                return true;
            }
            return false;
        }


    }
}
