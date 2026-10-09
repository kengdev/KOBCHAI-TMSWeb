using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator, Transport, Accounting, Super")]
public class PlaceController : Controller
{
    private readonly PlaceRepository _placeService;
    private readonly DropListRepository _dropListService;
    private string LoggedInUser => User.Identity.Name;

    public PlaceController(PlaceRepository placeService, DropListRepository dropListService)
    {
        _placeService = placeService;
        _dropListService= dropListService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Add()
    {
        ViewData["province_code"] = from a in await _dropListService.GetProvince() select new SelectListItem { Value = a.key.ToString(), Text = a.label };

        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> AddPlace(PlaceModel model)
    {
        string username = LoggedInUser;

        model.is_actived = true;
        model.is_enabled = true;
        model.create_by = username;
        model.place_lat = model.place_lat != null ? model.place_lat : "-";
        model.place_lon = model.place_lon != null ? model.place_lon : "-";
        model.place_remark = model.place_remark != null ? model.place_remark : "-";
        model.place_name = model.place_name.ToUpper();
        model.place_address = model.place_address.ToUpper();

        ResponseViewModel result = await _placeService.AddPlace(model);
        return Json(result);
    }

    public async Task<IActionResult> Edit(string code)
    {
        var data = await _placeService.GetPlaceByCode(code);
        ViewData["province_code"] = from a in await _dropListService.GetProvince() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.province_code };
        ViewData["zone_code"] = from a in await _dropListService.GetZone() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.zone_code };

        return PartialView(data);
    }

    public async Task<IActionResult> EditPlace(PlaceModel placeToEdit)
    {
        string username = LoggedInUser;
        placeToEdit.update_by = username;
        placeToEdit.place_remark = placeToEdit.place_remark != null ? placeToEdit.place_remark : "-";
        placeToEdit.place_name = placeToEdit.place_name.ToUpper();
        placeToEdit.place_address = placeToEdit.place_address.ToUpper();

        ResponseViewModel result = await _placeService.EditPlace(placeToEdit);
        return Json(result);
    }

    public async Task<IActionResult> GetPlaceDataByCode(string code)
    {
        var data = _placeService.GetPlaceDataByCode(code);
        return Json(data);
    }

    public async Task<IActionResult> GetPlaceDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _placeService.GetPlaceDataList();
        var data = query.Where(w => (iFilter.filterCustomer == null || w.place_port_code.ToUpper().Contains(iFilter.filterCustomer.ToUpper())) &&
        (iFilter.filterNameTH == null || w.place_name.ToUpper().Contains(iFilter.filterNameTH.ToUpper())) &&
        (iFilter.filterNameEN == null || w.zone_name.ToUpper().Contains(iFilter.filterNameEN.ToUpper())));

        Func<PlaceViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.place_port_code :
                                                            param.iSortCol_0 == 2 ? c.place_name :
                                                            param.iSortCol_0 == 3 ? c.zone_name : c.place_name);

        IEnumerable<PlaceViewModel> listQuery;
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

    public async Task<IActionResult> GetZoneByCode(string code)
    {
        var data = await _placeService.GetZoneDataByCode(code);

        return Json(data);
    }
    
    public async Task<IActionResult> Remove(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _placeService.Remove(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Active(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _placeService.Active(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Inactive(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _placeService.Inactive(code, username);
        return Json(result);
    }
}