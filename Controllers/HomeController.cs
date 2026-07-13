using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TMSWeb.Controllers;
[Authorize]
public class HomeController : Controller
{
    private string LoggedInUser => User.Identity.Name;
    public IActionResult Index()
    {
        ViewBag.Group = this.GetLoggedGroup();
        return View();
    }

    private string GetLoggedGroup()
    {
        var data = User.Claims.Where(w => w.Type == "group").FirstOrDefault();
        return data.Value;
    }
}
