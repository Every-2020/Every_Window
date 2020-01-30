using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetwork.Data
{
    public class TResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static implicit operator TResponse<T>(TResponse<string> v)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class Nothing
    {
        public static Nothing AtAll => null;
    }
}
