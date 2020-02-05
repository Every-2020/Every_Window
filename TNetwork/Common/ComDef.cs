using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetwork.Common
{
    public class ComDef
    {
        public enum LOGIN_STATS { DISCONNECT, CONNECT };

        public static string serverUrl { get; set; } //HOSTING

		public const int TIME_OUT = 30000;
    }
}
