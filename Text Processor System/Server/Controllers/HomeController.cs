using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Server.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }

        [HttpPost]
        public async Task<OkResult> Post(HttpRequestMessage request)
        {
            await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Ok();
        }
    }
}