using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AudioHost.Controllers
{
    public class UploadController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var storeLocation = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new FileStreamProvider(storeLocation);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}