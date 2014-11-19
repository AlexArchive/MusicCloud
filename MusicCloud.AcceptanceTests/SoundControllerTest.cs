using Xunit;

namespace MusicCloud.AcceptanceTests
{
    public class SoundControllerTest
    {
        [Fact]
        public void GetReturnsCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;
                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}