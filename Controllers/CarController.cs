using Microsoft.AspNetCore.Mvc;

namespace FastDrive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
