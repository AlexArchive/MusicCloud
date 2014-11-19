using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace MusicCloud.AcceptanceTests
{
    public class SoundControllerTest
    {
        [Fact]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;
                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        public void PostReturnsResponseWithCorrectStatusCode()
        {
            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(Resources.Song);
            imageContent.Headers.ContentType = 
                MediaTypeHeaderValue.Parse("image/jpg");
            requestContent.Add(imageContent, "image", "image.jpg");

            using (var client = HttpClientFactory.Create())
            {
                var response = client.PostAsync("", requestContent).Result;
                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}