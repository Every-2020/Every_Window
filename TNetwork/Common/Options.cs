using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Data;

namespace TNetwork.Common
{
    public static class Options
    {
        public static string serverUrl { get; set; } = null;
        public static string loginUrl { get; set; } = null;
        public static string logoutUrl { get; set; } = null;
        public static string tokenRefreshUrl { get; set; } = null;
        public static int timeOut { get; set; } = 30000;

        public static string id { get; set; } = null;
        public static string password { get; set; } = null;

        public static TokenInfo tokenInfo { get; set; } = null;

    }
}
