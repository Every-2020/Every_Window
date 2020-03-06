using TNetwork.Data;
using System.Threading.Tasks;
using RestSharp;

namespace TNetwork
{
    public interface IServerProtocol
    {
        bool CheckTokenExpired(IRestResponse response);
        #region 토큰
        Task TokenRefresh();
        #endregion
    }
}