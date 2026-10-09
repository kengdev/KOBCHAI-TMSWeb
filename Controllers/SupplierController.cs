using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator, Transport, Super")]
public class SupplierController : Controller
{
    private readonly SupplierRepository _supplierService;
    private readonly DropListRepository _dropListService;
    private string LoggedInUser => User.Identity.Name;

    public SupplierController(SupplierRepository supplierService,DropListRepository dropListService)
    {
        _supplierService = supplierService;
        _dropListService = dropListService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Add()
    {
        ViewData["SupplierTypeCode"] = from a in await _dropListService.GetSupplierType() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["BankName"] = from a in await _dropListService.GetBank() select new SelectListItem { Value = a.label.ToString(), Text = a.label };
        return View();
    }

    public async Task<IActionResult> Edit(string code)
    {
        var model = await _supplierService.GetSupplierData(code);
        //var fleet = await _supplierService.GetInternalFleetByCode(code);
        /*var supplier = await _supplierService.GetExternalFleetByCode(code);

        if (supplier != null)
            model.TblFleetExternals.Add(supplier);*/

        //if (fleet != null)
            //model.TblFleetInternals.Add(fleet);

        ViewData["BankName"] = from a in await _dropListService.GetBank() select new SelectListItem { Value = a.label.ToString(), Text = a.label, Selected = a.label == model.BankName };
        return View(model);
    }

    public async Task<IActionResult> AddSupplierData(TblSupplier model)
    {
        string username = LoggedInUser;

        model.BankId = model.BankId != null ? model.BankId : "-";
        model.BankName = model.BankName != null ? model.BankName : "-";
        model.IsActived = true;
        model.IsEnabled = true;
        model.CreateDate = DateTime.Now;
        model.CreateBy = username;
        model.UpdateDate = DateTime.Now;
        model.UpdateBy = username;

        ResponseViewModel result = await _supplierService.Add(model);
        return Json(result);
    }

    public async Task<IActionResult> EditSupplierData(TblSupplier model)
    {
        string username = LoggedInUser;

        model.BankId = model.BankId != null ? model.BankId : "-";
        model.BankName = model.BankName != null ? model.BankName : "-";
        model.UpdateDate = DateTime.Now;
        model.UpdateBy = username;

        ResponseViewModel result = await _supplierService.Edit(model);
        return Json(result);
    }

    public async Task<IActionResult> GetSupplierDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _supplierService.GetSupplierDataList();
        var data = query.Where(w => (iFilter.filterNameTH == null || w.supplier_name.ToUpper().Contains(iFilter.filterNameTH.ToUpper())) &&
        (iFilter.filterPhone == null || w.supplier_phone.ToUpper().Contains(iFilter.filterPhone.ToUpper())));

        Func<SupplierViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.supplier_name :
                                                                    param.iSortCol_0 == 1 ? c.supplier_name : c.supplier_name);

        IEnumerable<SupplierViewModel> listQuery;
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

    public async Task<IActionResult> Remove(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _supplierService.Remove(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Active(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _supplierService.Active(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Inactive(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _supplierService.Inactive(code, username);
        return Json(result);
    }
}