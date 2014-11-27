using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
        public HttpResponseMessage Get()
        {
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
                        SELECT TOP 1
                            Audio.PathName() AS soundPath,
                            GET_FILESTREAM_TRANSACTION_CONTEXT() as transactionContext
                        FROM [dbo].[Sound]
                        WHERE Slug = 'Foo'";
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
                        using (var source =
                            new SqlFileStream(soundPath, transactionContext, FileAccess.Read))
                        {
                            var destination = new MemoryStream();
                            source.CopyTo(destination);
                            destination.Position = 0;
                            var response = Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StreamContent(destination);
                            return response;
                        }
                    }
                }
            }
        }

        public async Task<HttpResponseMessage> Post()
        {
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
                            var contentProvider = await Request.Content.ReadAsMultipartAsync();
                            var sound = contentProvider.Contents.First();

                            sound.CopyToAsync(destination).Wait();
                        }
                        command.Transaction.Commit();
                    }
                }

                //var contentProvider = await Request.Content.ReadAsMultipartAsync();
                //sound = await contentProvider.Contents.First().ReadAsStreamAsync();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }
    }
}