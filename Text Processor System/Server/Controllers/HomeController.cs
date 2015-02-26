using System.Web.Http;
using System.Web.Http.Results;

namespace Server.Controllers
{
    internal class HomeController : ApiController
    {
        [HttpPost]
        public OkResult Index()
        {
            return Ok();
        }
    }
}