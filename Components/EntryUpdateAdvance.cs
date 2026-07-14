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
public class EntryUpdateAdvance : ViewComponent
{
    private string LoggedInUser => User.Identity.Name;
    private readonly JobRepository _jobService;

    public EntryUpdateAdvance(JobRepository jobService)
    {
        _jobService = jobService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string code, bool lift_cash)
    {
        var advance = await _jobService.GetAdvanceByJob(code);
        var advance_other = await _jobService.GetAdvanceOtherByJob(advance.AdvanceCode);

        ViewBag.Other = advance_other;
        return View(advance);
    }
}