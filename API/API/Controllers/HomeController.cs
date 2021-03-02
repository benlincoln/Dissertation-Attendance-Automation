using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
