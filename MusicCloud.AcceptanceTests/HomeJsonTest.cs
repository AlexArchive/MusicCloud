using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using Xunit;

namespace MusicCloud.AcceptanceTests
{
    public class HomeJsonTest
    {
        [Fact]
        public void GetReturnsNotFound()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("/foo").Result;
                Assert.Equal(
                    HttpStatusCode.NotFound, 
                    response.StatusCode);
            }
        }

        [Fact]
        public void GetReturnsJsonContent()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("/foo").Result;
                Assert.Equal(
                    "application/json",
                    response.Content.Headers.ContentType.MediaType);
                var json = response.Content
                    .ReadAsStringAsync()
                    .ContinueWith(t => JsonConvert.DeserializeObject(t.Result)).Result;
                Assert.NotNull(json);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetReturnsCorrectResult()
        {
            // Setup
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        INSERT INTO [dbo].[Sound]
                        SELECT NEWID(), 'Foo', 'Foo', 0x;";
                    command.ExecuteNonQuery();
                }
            }

            using (var client = HttpClientFactory.Create())
            {
                // Exercise
                var response = client.GetAsync("/foo").Result;
                var actual = response.Content
                    .ReadAsStringAsync()
                    .ContinueWith(t => JsonConvert.DeserializeObject(t.Result))
                    .Result
                    .ToString();
                var sound = new SoundModel
                {
                    Title = "Foo",
                    SoundLink = "Sound/foo"
                };
                var expected = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(sound)).ToString();

                // Verify
                Assert.Contains(expected, actual);
            }
        }
    }
}