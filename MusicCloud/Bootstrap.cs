using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;

namespace MusicCloud
{
    public static class Bootstrap
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}",
                defaults: new
                {
                    controller = "Sound"
                });
        }

        public static void InstallDatabase()
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionStr);
            builder.InitialCatalog = "Master";

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    var schema = Properties.Resources.DatabaseSchema;
                    foreach (var statement in 
                        schema.Split(new[] {"GO"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        command.CommandText = statement;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void UninstallDatabase()
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["MusicCloud"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionStr);
            builder.InitialCatalog = "Master";

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        IF DB_ID('MusicCloud') IS NOT NULL BEGIN
	                        DROP DATABASE [MusicCloud]
                        END";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
