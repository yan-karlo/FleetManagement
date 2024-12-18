using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.WebUI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("home")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("errors")]
        public IActionResult Error()
        { 
            return View(); 
        }
    }
}
