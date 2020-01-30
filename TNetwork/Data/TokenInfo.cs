using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetwork.Data
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public TokenInfo(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public TokenInfo()
        {

        }
    }
}
