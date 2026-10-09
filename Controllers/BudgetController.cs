using Microsoft.AspNetCore.Mvc;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator, Accounting, Super")]
public class BudgetController : Controller
{
    private readonly DbTMSContext _context;
    private string LoggedInUser => User.Identity.Name;

    public BudgetController(DbTMSContext context)
    {
        _context = context;
    }

    #region Transport Budget
    public IActionResult Index()
    {
        //var _service = new BudgetRepository(_context);
        //var model = _service.GetTrBudgetData();

        return View();
    }

    public IActionResult _AddBalance()
    {
        return PartialView();
    }

    public IActionResult _AddPayment()
    {
        return PartialView();
    }

    public IActionResult TransactionDetail(string code)
    {
        /*var _service = new BudgetRepository(_context);
        var list = _service.GetBudgetTransactionDetail(code);
        var data = _service.GetBudgetTransactionDataByCode(code);

        ViewBag.List = list;*/
        return View();
    }

    public async Task<IActionResult> Balance()
    {
        var budgetService = new BudgetRepository(_context);
        var data = await budgetService.GetAdvanceFundData();

        return View(data);
    }

    /*[HttpPost]
    public async Task<IActionResult> SetBalanceData(decimal amount, DateTime budget_date, string remark)
    {
        var _notif = new LineNotifRepository();
        var _service = new BudgetRepository(_context);

        ResponseViewModel response = _service.UpdateBalance(amount, budget_date, remark, this.LoggedInUser);
        if (response.result)
            await _notif.NotifGroup(response.notifMessage);

        Thread.Sleep(2000);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddPaymentData(decimal amount, DateTime budget_date, string remark)
    {
        var _notif = new LineNotifRepository();
        var _service = new BudgetRepository(_context);

        ResponseViewModel response = _service.AddPayment(amount, budget_date, remark, this.LoggedInUser);
        if (response.result)
            await _notif.NotifGroup(response.notifMessage);

        Thread.Sleep(2000);
        return Json(response);
    }
    #endregion

    #region General Budget
    public IActionResult SetGeBalance()
    {
        var _service = new BudgetRepository(_context);
        var data = _service.GetGeBudget();

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> SetGeneralBalanceData(decimal amount, DateTime budget_date, string remark)
    {
        var _notif = new LineNotifRepository();
        var _service = new BudgetRepository(_context);

        ResponseViewModel response = _service.UpdateGeneralBalance(amount, budget_date, remark, this.LoggedInUser);
        if (response.result)
            await _notif.NotifGroup(response.notifMessage);

        Thread.Sleep(2000);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddGeneralPaymentData(decimal amount, DateTime budget_date, string remark)
    {
        var _notif = new LineNotifRepository();
        var _service = new BudgetRepository(_context);

        ResponseViewModel response = _service.AddGeneralPayment(amount, budget_date, remark, this.LoggedInUser);
        if (response.result)
            await _notif.NotifGroup(response.notifMessage);

        Thread.Sleep(2000);
        return Json(response);
    }*/
    #endregion
}