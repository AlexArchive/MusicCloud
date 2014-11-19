using System.Net.Http;
using System.Web.Http;

namespace MusicCloud
{
    public class SoundController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse();
        }
    }
}