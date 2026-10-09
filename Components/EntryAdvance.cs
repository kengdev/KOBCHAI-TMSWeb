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
public class EntryAdvance : ViewComponent
{
    private readonly JobRepository _jobService;

    public EntryAdvance(JobRepository jobService)
    {
        _jobService = jobService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string code)
    {
        var cashflow = await _jobService.GetAdvanceByJob(code);

        return View(cashflow);
    }
}