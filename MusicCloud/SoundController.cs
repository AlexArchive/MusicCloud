using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MusicCloud
{
    public class SoundController : ApiController
    {
        private static Stream sound;

        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(sound);
            return response;
        }

        public async Task<HttpResponseMessage> Post()
        {
            var contentProvider = await Request.Content.ReadAsMultipartAsync();
            sound = await contentProvider.Contents.First().ReadAsStreamAsync();

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}