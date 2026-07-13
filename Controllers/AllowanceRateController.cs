using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator")]
public class AllowanceRateController : Controller
{
    private readonly RateRepository _rateService;
    private readonly DropListRepository _dropListService;
    private string LoggedInUser => User.Identity.Name;

    public AllowanceRateController(RateRepository rateService, DropListRepository dropListService)
    {
        _rateService = rateService;
        _dropListService = dropListService;
    }

    public IActionResult DriverRate()
    {
        return View();
    }

    public async Task<IActionResult> GetDriverAllowanceDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _rateService.GetDriverRate();
        var data = query.Where(w => (iFilter.filterMission == null || w.name.ToUpper().Contains(iFilter.filterMission.ToUpper())) &&
        (iFilter.filterZone == null || w.zone_name.ToUpper().Contains(iFilter.filterZone.ToUpper())));

        Func<DriverRateViewModel, string> orderingFunction = (c => param.iSortCol_0 == 0 ? c.name :
        param.iSortCol_0 == 1 ? c.zone_name : c.name);

        IEnumerable<DriverRateViewModel> listQuery;
        if (param.sSortDir_0 == "asc")
        {
            listQuery = data.OrderBy(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }
        else
        {
            listQuery = data.OrderByDescending(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }

        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = listQuery
        });
    }

    public async Task<IActionResult> EditDriverRate(decimal value, string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _rateService.EditDriverRate(value, code, username);
        return Json(result);
    }

    public async Task<IActionResult> SupplyerRate()
    {
        ViewData["supplyer_code"] = from a in await _dropListService.GetSupplierTransport() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        return View();
    }

    public async Task<IActionResult> GetSupplierRateDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var data = await _rateService.GetSupplyerRate(iFilter.filterSupplier);
        data = data.Where(w => (iFilter.filterMission == null || w.place_name.ToLower().Contains(iFilter.filterMission.ToLower())) &&
                                    (iFilter.filterZone == null || w.zone_name.ToLower().Contains(iFilter.filterZone.ToLower()))).ToList();

        Func<SupplyerRateViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.place_name :
        param.iSortCol_0 == 2 ? c.zone_name : c.place_name);

        IEnumerable<SupplyerRateViewModel> listQuery;
        if (param.sSortDir_0 == "asc")
        {
            listQuery = data.OrderBy(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }
        else
        {
            listQuery = data.OrderByDescending(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }

        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = listQuery
        });
    }

    public async Task<IActionResult> EditSupplierRate(decimal value, string supplier_code, string place_code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _rateService.EditSupplyerRate(value, supplier_code, place_code, username);
        return Json(result);
    }

    public async Task<IActionResult> FleetRate()
    {
        ViewData["supplyer_code"] = from a in await _dropListService.GetFleet() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        return View();
    }

    public async Task<IActionResult> GetFleetRateDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var data = await _rateService.GetFleetRate(iFilter.filterSupplier);
        data = data.Where(w => (iFilter.filterMission == null || w.place_name.ToLower().Contains(iFilter.filterMission.ToLower())) &&
                                    (iFilter.filterZone == null || w.zone_name.ToLower().Contains(iFilter.filterZone.ToLower()))).ToList();

        Func<SupplyerRateViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.place_name :
        param.iSortCol_0 == 2 ? c.zone_name : c.supplyer_name);

        IEnumerable<SupplyerRateViewModel> listQuery;
        if (param.sSortDir_0 == "asc")
        {
            listQuery = data.OrderBy(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }
        else
        {
            listQuery = data.OrderByDescending(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
        }

        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = listQuery
        });
    }

    public async Task<IActionResult> EditFleetRate(decimal value, string supplier_code, string place_code, string type)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _rateService.EditFleetRate(value, supplier_code, place_code, type, username);
        return Json(result);
    }
}