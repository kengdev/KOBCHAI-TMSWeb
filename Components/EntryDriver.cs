using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace TMSWeb.Components;
[Authorize]
public class EntryDriver : ViewComponent
{
    private string LoggedInUser => User.Identity.Name;
    private readonly TruckRepository _truckService;

    public EntryDriver(TruckRepository truckService)
    {
        _truckService = truckService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string driver_code)
    {
        var model = new TblTruckDriver();
        var query = await _truckService.GetDriverByDriverCode(driver_code);

        model.DriverCode = query.DriverCode;
        ViewBag.Driver = query;
        return View(model);
    }
}