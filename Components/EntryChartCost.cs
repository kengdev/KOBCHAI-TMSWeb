using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using TMSWeb.Repositories;
using TMSWeb.Services;
using TMSWeb.Models.Entities.LOG;

namespace Dummy.Components;
public class EntryChartCost : ViewComponent
{
    private readonly DbTMSContext db;
    private readonly DashboardRepository _dashboardService;

    public EntryChartCost(DbTMSContext context, DashboardRepository dashboardService)
    {
        db = context;
        _dashboardService = dashboardService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string month = DateTime.Now.Month.ToString();
        string year = DateTime.Now.Year.ToString();
        var data = await _dashboardService.GetJobCostCustomerBySupplier(month, year);
        return View(data);
    }
}