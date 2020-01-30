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

        //소켓 서버 주소
        public static string serverUrl { get; set; }//HOSTING
        public const string LOCALSERVER1_URL = "http://10.80.161.236:3000"; //정한나
        public const string LOCALSERVER2_URL = "http://10.80.161.145:3000"; //김시아
        //public const string LOCALSERVER2_URL = "http://192.168.0.14:3000"; //김시아
        public const string LOCALSERVER3_URL = "http://10.80.161.225:3000"; //박태형

		public const int TIME_OUT = 30000;

        public static string DEFAULT_PROFILEIMAGE = @"/Assets/default.png";
        public static string DEFAULT_NOIMAGE = "/Assets/nophoto.jpg";
        public static string DEFAULT_GROUPIMAGE = "/Assets/group.png";
    }
}
