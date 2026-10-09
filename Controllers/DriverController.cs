using Microsoft.AspNetCore.Mvc;

namespace TMSWeb.Controllers
{
    public class DriverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
