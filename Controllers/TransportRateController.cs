
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator")]
public class TransportRateController : Controller
{
    private readonly TransportRateRepository _rateService;
    private string LoggedInUser => User.Identity.Name;

    public TransportRateController(TransportRateRepository rateService)
    {
        _rateService = rateService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    public async Task<IActionResult> AddCost(TblCustomerCost costToAdd, decimal supplyer_cost, decimal driver_cost)
    {
        string username = LoggedInUser;

        costToAdd.CostRemark = costToAdd.CostRemark != null ? costToAdd.CostRemark : "-";
        costToAdd.IsActived = true;
        costToAdd.IsEnabled = true;
        costToAdd.CreateDate = DateTime.Now;
        costToAdd.CreateBy = username;
        costToAdd.UpdateDate = DateTime.Now;
        costToAdd.UpdateBy = username;
        costToAdd.ReceiptName = costToAdd.ReceiptName.ToUpper();
        costToAdd.ReceiptAddress = costToAdd.ReceiptAddress.ToUpper();

        ResponseViewModel result = await _rateService.AddCost(costToAdd, supplyer_cost, driver_cost);
        return Json(result);
    }

    public async Task<IActionResult> Edit(string code)
    {
        var data_cost = await _rateService.GetCustomerCostDataByCode(code);
        var data_customer = await _rateService.GetCustomerDataByCode(data_cost.CostCode);
        ViewBag.Customer = data_customer;
        return View(data_cost);
    }

    [HttpPost]
    public async Task<IActionResult> EditCost(TblCustomerCost costToEdit)
    {
        string username = LoggedInUser;

        costToEdit.CostRemark = costToEdit.CostRemark != null ? costToEdit.CostRemark : "-";
        costToEdit.UpdateDate = DateTime.Now;
        costToEdit.UpdateBy = username;
        costToEdit.ReceiptName = costToEdit.ReceiptName.ToUpper();
        costToEdit.ReceiptAddress = costToEdit.ReceiptAddress.ToUpper();

        ResponseViewModel result = await _rateService.EditCost(costToEdit);
        return Json(result);
    }

    public IActionResult _SearchCost()
    {
        return View();
    }

    public async Task<IActionResult> GetCustomerCostDataByCode(string code)
    {
        var data = await _rateService.GetCustomerCostDataByCode(code);

        return Json(data);
    }

    public async Task<IActionResult> IsDuplicate(string customer_code, string place_code)
    {
        var result = await _rateService.IsDuplicate(customer_code, place_code);

        return Json(result);
    }

    public async Task<IActionResult> Remove(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _rateService.Remove(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Active(string code)
    {
        string username = LoggedInUser;
  
        ResponseViewModel result = await _rateService.Active(code, username);
        return Json(result);
    }

    public async Task<IActionResult> Inactive(string code)
    {
        string username = LoggedInUser;

        ResponseViewModel result = await _rateService.Inactive(code, username);
        return Json(result);
    }

    public async Task<IActionResult> GetCostDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _rateService.GetCostDataList();
        var data = query.Where(w => 
        (iFilter.filterNameEN == null || w.mission_name.ToUpper().Contains(iFilter.filterNameEN.ToUpper())) &&
        (iFilter.filterCustomer == null || w.customer_name_th.ToUpper().Contains(iFilter.filterNameEN.ToUpper())) &&
        (iFilter.filterNameTH == null || w.job_name.ToUpper().Contains(iFilter.filterNameTH.ToUpper())));

        Func<CostViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.customer_name_th :
                                                                param.iSortCol_0 == 2 ? c.mission_name : c.customer_name_th);

        IEnumerable<CostViewModel> listQuery;
        if (param.sSortDir_0 == "asc")
        {
            if (param.iSortCol_0 == 4)
            {
                listQuery = data.OrderBy(o => o.cost_value)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
            else
            {
                listQuery = data.OrderBy(orderingFunction)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
            }
        }
        else
        {
            if (param.iSortCol_0 == 4)
            {
                listQuery = data.OrderByDescending(o => o.cost_value)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
            else
            {
                listQuery = data.OrderByDescending(orderingFunction)
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);
            }
        }

        return Json(new
        {
            sEcho = param.sEcho,
            iTotalRecords = data.Count(),
            iTotalDisplayRecords = data.Count(),
            aaData = listQuery
        });
    }
}