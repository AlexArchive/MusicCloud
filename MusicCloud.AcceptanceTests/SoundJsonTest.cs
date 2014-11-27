using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;

namespace MusicCloud.AcceptanceTests
{
    public class SoundJsonTest
    {
        [Fact]
        [UseDatabase]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            // Setup
            var expected = Resources.Song;
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            var transactionContext = new byte[0];
            var soundPath = string.Empty;
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        INSERT INTO [dbo].[Sound]
                        OUTPUT 
                            GET_FILESTREAM_TRANSACTION_CONTEXT() AS transactionContext, 
                            inserted.Audio.PathName() AS soundPath
                        SELECT NEWID(), 'Foo', 'Foo', 0x;";
                    using (var transaction = connection.BeginTransaction())
                    {
                        command.Transaction = transaction;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactionContext = (byte[])reader["transactionContext"];
                                soundPath = reader["soundPath"].ToString();
                            }
                        }
                        using (var destination =
                            new SqlFileStream(soundPath, transactionContext, FileAccess.Write))
                        {
                            destination.WriteAsync(expected, 0, expected.Length).Wait();
                        }
                        command.Transaction.Commit();
                    }
                }
            }

            using (var client = HttpClientFactory.Create())
            {
                // Exercise
                var response = client.GetAsync("Sound").Result;

                // Verify
                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void PostSoundSucceeds()
        {
            var requestContent = new MultipartFormDataContent();
            var soundContent = new ByteArrayContent(Resources.Song);
            requestContent.Add(soundContent, "Song", "Song.mp3");

            using (var client = HttpClientFactory.Create())
            {
                var response = client.PostAsync("Sound", requestContent).Result;

                Assert.True(
                    response.StatusCode == HttpStatusCode.Created,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void AfterPostingSoundGetReturnsPostedSound()
        {
            var expected = Resources.Song;
            var requestContent = new MultipartFormDataContent();
            var soundContent = new ByteArrayContent(expected);
            requestContent.Add(soundContent, "Song", "Song.mp3");

            using (var client = HttpClientFactory.Create())
            {
                client.PostAsync("Sound", requestContent).Wait();
                var actual = client.GetByteArrayAsync("Sound").Result;
                
                Assert.Equal(expected.Length, actual.Length);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetReturnsCorrectSoundFromDatabase()
        {
            // Setup
            var expected = Resources.Song;
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            var transactionContext = new byte[0];
            var soundPath = string.Empty;
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        INSERT INTO [dbo].[Sound]
                        OUTPUT 
                            GET_FILESTREAM_TRANSACTION_CONTEXT() AS transactionContext, 
                            inserted.Audio.PathName() AS soundPath
                        SELECT NEWID(), 'Foo', 'Foo', 0x;";
                    using (var transaction = connection.BeginTransaction())
                    {
                        command.Transaction = transaction;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactionContext = (byte[])reader["transactionContext"];
                                soundPath = reader["soundPath"].ToString();
                            }
                        }
                        using (var destination =
                            new SqlFileStream(soundPath, transactionContext, FileAccess.Write))
                        {
                            destination.WriteAsync(expected, 0, expected.Length).Wait();
                        }
                        command.Transaction.Commit();
                    }
                }
            }

            using (var client = HttpClientFactory.Create())
            {
                // Exercise
                var actual = client.GetByteArrayAsync("Sound").Result;

                // Verify
                Assert.Equal(expected.Length, actual.Length);
            }
        }
    }
}