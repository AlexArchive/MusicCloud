using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicCloud
{
    public class HomeController : ApiController
    {
        public HttpResponseMessage Get(string soundName)
        {
            var model = new SoundModel();
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT TOP 1 *
                        FROM [dbo].[Sound]";

                    using (var reader = command.ExecuteReader())
                    {
                        var found = reader.Read();
                        model.Title = reader["Title"].ToString();
                        model.SoundLink = "Sound/" + soundName;
                    }
                }
            }

            return Request.CreateResponse(model);
        }
    }
}
