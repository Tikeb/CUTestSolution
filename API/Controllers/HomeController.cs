using Common.Models.Responses.Home;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/hello/{name}")]
    public class HomeController : Controller
    {
        public IActionResult Hello(string name)
        {
            HelloResponse helloResponse = new HelloResponse
            {
                Message = $"Hello {name} from the API.",
            };

            return this.Ok(helloResponse);
        }
    }
}