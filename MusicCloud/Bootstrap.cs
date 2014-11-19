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
    }
}
