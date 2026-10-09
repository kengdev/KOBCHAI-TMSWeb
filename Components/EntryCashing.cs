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
public class EntryCashing : ViewComponent
{
    private readonly JobRepository _jobService;

    public EntryCashing(JobRepository jobService)
    {
        _jobService = jobService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string code)
    {
        var cashflow = await _jobService.GetCashingByJob(code);

        return View(cashflow);
    }
}