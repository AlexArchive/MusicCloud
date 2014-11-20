using System.Net;
using System.Net.Http;
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
        public void PostReturnsResponseWithCreatedStatusCode()
        {
            var requestContent = new MultipartFormDataContent();
            var soundContent = new ByteArrayContent(Resources.Song);
            requestContent.Add(soundContent, "Song", "Song.mp3");

            using (var client = HttpClientFactory.Create())
            {
                var response = client.PostAsync("", requestContent).Result;

                Assert.True(
                    response.StatusCode == HttpStatusCode.Created,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetAfterPostReturnsPostedSound()
        {
            var expected = Resources.Song;
            var requestContent = new MultipartFormDataContent();
            var soundContent = new ByteArrayContent(expected);
            requestContent.Add(soundContent, "Song", "Song.mp3");

            using (var client = HttpClientFactory.Create())
            {
                client.PostAsync("", requestContent).Wait();

                var actual = client.GetByteArrayAsync("").Result;
                Assert.Equal(expected.Length, actual.Length);
            }
        }
    }
}