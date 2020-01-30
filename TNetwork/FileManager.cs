using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Data;
using TNetwork.Model;

namespace TNetwork
{
    public partial class NetworkManager
    {
        public string FILE_URL = "/upload/{0}";
        private readonly Encoding encoding = Encoding.UTF8;

        private string GetFormattedFileUrl(string extension)
        {
            return string.Format(FILE_URL, extension);
        }

        public async Task<IRestResponse> UploadFile(Dictionary<string, object> postParameters, string extension)
        {
            string formDataBoundary = String.Format("---------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return await PostFormAsync(GetFormattedFileUrl(extension), contentType, formData, extension);
        }

        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new MemoryStream();

            foreach (var param in postParameters)
            {
                formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                if (param.Value is MultiPartFile)
                {
                    MultiPartFile fileToUpload = (MultiPartFile)param.Value;

                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);

                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        "name",
                        fileToUpload.Guid);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);

            formDataStream.Close();

            return formData;
        }
    }
}
