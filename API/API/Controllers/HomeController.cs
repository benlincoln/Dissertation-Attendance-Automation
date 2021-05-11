using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string returnTest()
        {
            return "If you are seeing this, the API is successfully running";
        }
        
    }
}
