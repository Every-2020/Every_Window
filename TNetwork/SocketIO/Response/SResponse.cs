using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetwork.SocketIO.Response
{
    public class SResponse<T>
    {
        public int Status { get; set; }
        public T Data { get; set; }
    }
}
