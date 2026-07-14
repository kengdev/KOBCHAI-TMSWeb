using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace Dummy.Components;
[Authorize]
public class EntryUpdateCashing : ViewComponent
{
    private string LoggedInUser => User.Identity.Name;
    private readonly JobRepository _jobService;

    public EntryUpdateCashing(JobRepository jobService)
    {
        _jobService = jobService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string code)
    {
        var cashing = await _jobService.GetCashingByJob(code);

        return View(cashing);
    }
}