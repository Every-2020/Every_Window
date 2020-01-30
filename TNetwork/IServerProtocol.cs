using TNetwork.Data;
using System.Threading.Tasks;

namespace TNetwork
{
    public interface IServerProtocol
    {
        #region 토큰
        Task TokenRefresh();
        #endregion
    }
}