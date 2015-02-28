using System.Web.Http;
using Owin;

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi", "{controller}/{action}/{id}", new {id = RouteParameter.Optional});

            appBuilder.UseWebApi(config);
        }
    }
}