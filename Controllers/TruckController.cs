using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator, Transport, Super")]
public class TruckController : Controller
{
    private readonly TruckRepository _truckService;
    private readonly DropListRepository _dropListService;
    private string LoggedInUser => User.Identity.Name;

    public TruckController(TruckRepository truckService, DropListRepository dropListService)
    {
        _truckService = truckService;
        _dropListService = dropListService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["brand_code"] = from a in await _dropListService.GetTruckBrand() select new SelectListItem { Value = a.label.ToString(), Text = a.label };
        ViewData["fleet_code"] = from a in await _dropListService.GetFleet() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["truck_type_code"] = from a in await _dropListService.GetTruckType() select new SelectListItem { Value = a.label.ToString(), Text = a.label };
        return View();
    }

    public async Task<IActionResult> Add()
    {
        ViewData["brand_code"] = from a in await _dropListService.GetTruckBrand() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["fuel_type_code"] = from a in await _dropListService.GetFuelType() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["truck_type_code"] = from a in await _dropListService.GetTruckType() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["truck_year"] = from a in _dropListService.GetTruckYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label };

        return PartialView();
    }

    public async Task<IActionResult> AddTruck(IFormFile files, TruckModel truckToAdd)
    {
        string username = LoggedInUser;

        truckToAdd.truck_img = "dfimage.jpg";
        if (files != null)
        {
            string file_name = truckToAdd.truck_code + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\pics\\trucks", file_name);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }
            truckToAdd.truck_img = file_name;
        }
        else
        {
            truckToAdd.truck_img = "default.jpg";
        }

        ResponseViewModel result = await _truckService.AddTruck(truckToAdd, username);
        return Json(result);
    }

    public async Task<IActionResult> Edit(string code)
    {
        var data = await _truckService.GetTruckDataByCode(code);

        ViewData["brand_code"] = from a in await _dropListService.GetTruckBrand() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.brand_code };
        ViewData["fuel_type_code"] = from a in await _dropListService.GetFuelType() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.fuel_type_code };
        ViewData["truck_type_code"] = from a in await _dropListService.GetTruckType() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.truck_type_code };
        ViewData["truck_year"] = from a in _dropListService.GetTruckYear() select new SelectListItem { Value = a.key.ToString(), Text = a.label, Selected = a.key == data.truck_year };

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> EditTruck(IFormFile files, TruckModel truckToEdit)
    {
        string username = LoggedInUser;
        if (files != null)
        {
            string file_name = truckToEdit.truck_code + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\pics\\trucks", file_name);
            using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }

            truckToEdit.truck_img = file_name;
        }

        ResponseViewModel result = await _truckService.EditTruck(truckToEdit, username);
        return Json(result);
    }

    public async Task<IActionResult> EditDriver(string code)
    {
        var truck = await _truckService.GetTruckDataByCode(code);
        var driver = await _truckService.GetDriverListByTruck(code);
        var model = new TblTruck();
        model.TruckCode = truck.truck_code;

        ViewBag.DriverList = driver;
        ViewBag.Truck = truck;
        ViewData["driver_code"] = from a in await _dropListService.GetDriver() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        return View(model);
    }

    public async Task<IActionResult> EditDriverData(TblTruck model)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _truckService.EditTruckDriver(model, username);
        return Json(result);
    }

    public async Task<IActionResult> AddInsurance(string code)
    {
        TblTruckInsurance model = new TblTruckInsurance();
        model.TruckCode = code;

        var data = await _truckService.GetTruckDataByCode(code);
        ViewBag.Data = data;
        ViewData["InsuranceCode"] = from a in await _dropListService.GetInsurance() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["InsuranceTypeCode"] = from a in await _dropListService.GetInsuranceType() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        return View(model);
    }

    /*public IActionResult AddInsuranceData(TblTruckInsurance model)
    {
        var _service = new TruckRepository(_context);

        model.InsuranceDetail = model.InsuranceDetail != null ? model.InsuranceDetail : "-";
        model.IsEnabled = true;
        model.CreateDate = DateTime.Now;
        model.CreateBy = this.LoggedInUser;
        model.UpdateDate = DateTime.Now;
        model.UpdateBy = this.LoggedInUser;

        ResponseViewModel result = _service.AddInsuranceData(model);
        return Json(result);
    }*/

    public async Task<IActionResult> EditInsurance(string truck_license_insurance)
    {

        ViewData["insurance"] = from a in await _dropListService.GetInsurance() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["insurance_type"] = from a in await _dropListService.GetInsuranceType() select new SelectListItem { Value = a.key.ToString(), Text = a.label };

        var truck = await _truckService.GetTruckByLicense(truck_license_insurance);
        //var insurance = trk.GetInsuranceByLicense(truck_license_insurance);

        ViewBag.Truck = truck;
        //ViewBag.InsuranceList = insurance;
        return View();
    }

    /*public IActionResult EditTruckInsuranceData(TblTruck TruckInsuranceToEdit)
    {
        string username = LoggedInUser;
        var trk = new TruckRepository(_context);

        ResponseViewModel result = trk.EditTruckInsurance(TruckInsuranceToEdit, username);
        return Json(result);
    }*/

    public async Task<IActionResult> TruckDetail(string code)
    {
        string username = LoggedInUser;

        var truck = await _truckService.GetTruckByLicense(code);
        var driver = await _truckService.GetDriverListByTruck(code);

        ViewBag.Truck = truck;
        ViewBag.DriverList = driver;
        return View();
    }

    public async Task<IActionResult> GetTruckDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _truckService.GetTruckDataList();
        var data = query.Where(w => (iFilter.filterLicense == null || w.truck_license.ToUpper().Contains(iFilter.filterLicense.ToUpper())) &&
        (iFilter.filterNameTH == null || w.truck_no.ToUpper().Contains(iFilter.filterNameTH.ToUpper())) &&
        (iFilter.filterTruckGroup == null || w.truck_type_name.Contains(iFilter.filterTruckGroup)) &&
        (iFilter.filterBrand == null || w.brand_name.Contains(iFilter.filterBrand)) &&
        (iFilter.filterSupplier == null || w.fleet_supplier_type_code.Contains(iFilter.filterSupplier)) &&
        (iFilter.filterNameEN == null || w.truck_chassis_number.ToUpper().Contains(iFilter.filterNameEN.ToUpper())));

        Func<TruckViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.truck_no :
        param.iSortCol_0 == 2 ? c.truck_license :
        param.iSortCol_0 == 3 ? c.truck_type_name :
        param.iSortCol_0 == 4 ? c.brand_name :
        param.iSortCol_0 == 5 ? c.fleet_supplier_type_name : c.truck_no);

        IEnumerable<TruckViewModel> listQuery;
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

    public IActionResult EntryTruckDriver(string driver_code)
    {
        return ViewComponent("EntryDriver", driver_code);
    }

    public async Task<IActionResult> Remove(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _truckService.Remove(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Active(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _truckService.Active(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Inactive(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _truckService.Inactive(code, username);
        return Json(result);
    }

    private string GetFileType(string name)
    {
        string[] lines = name.Split('.');
        var count_lines = lines.Count();
        return lines[count_lines - 1];
    }

}