using System.Net.Http;
using System.Net.Http.Headers;

namespace AudioHost
{
    public class FileStreamProvider : MultipartFormDataStreamProvider
    {
        public FileStreamProvider(string rootPath) 
            : base(rootPath)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = base.GetLocalFileName(headers);
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}