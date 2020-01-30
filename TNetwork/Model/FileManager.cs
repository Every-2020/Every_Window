using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNetwork.Model
{
    public class MultiPartFile
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Guid { get; set; }
        public MultiPartFile(byte[] file) : this(file, null) { }
        public MultiPartFile(byte[] file, string filename) : this(file, filename, null) { }
        public MultiPartFile(byte[] file, string filename, string contentType) : this(file, filename, contentType, null) { }
        public MultiPartFile(byte[] file, string filename, string contenttype, string guid)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
            Guid = guid;
        }

    }
}
