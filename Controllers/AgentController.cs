using Microsoft.AspNetCore.Mvc;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TMSWeb.Controllers;
[Authorize(Roles = "Administrator, Transport, Super")]
public class AgentController : Controller
{
    private readonly AgentRepository _agentService;
    private string LoggedInUser => User.Identity.Name;

    public AgentController(AgentRepository agentService)
    {
        _agentService = agentService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetAgentDataList(jQueryDataTableParamModel param, iDataTableParamModel iFilter)
    {
        var query = await _agentService.GetAgentDataList();
        var data = query.Where(w => (iFilter.filterNameTH == null || w.name.ToUpper().Contains(iFilter.filterNameTH.ToUpper())));

        Func<AgentViewModel, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.name : c.name);

        IEnumerable<AgentViewModel> listQuery;
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

    public IActionResult Add()
    {
        return View();
    }

    public async Task<IActionResult> AddData(TblAgent model)
    {
        model.IsActived = true;
        model.IsEnabled = true;
        model.CreateDate = DateTime.Now;
        model.CreateBy = this.LoggedInUser;
        model.UpdateDate = DateTime.Now;
        model.UpdateBy = this.LoggedInUser;

        ResponseViewModel result = await _agentService.AddData(model);
        return Json(result);
    }

    public async Task<IActionResult> Edit(string code)
    {
        var model = await _agentService.GetAgentDataByCode(code);
        return View(model);
    }

    public async Task<IActionResult> EditData(TblAgent dataToEdit)
    {
        var username = LoggedInUser;
        dataToEdit.UpdateDate = DateTime.Now;
        dataToEdit.UpdateBy = username;

        ResponseViewModel result = await _agentService.EditAgentData(dataToEdit);
        return Json(result);
    }

    public async Task<IActionResult> Remove(string code)
    {
        var username = LoggedInUser;
 
        ResponseViewModel result = await _agentService.Remove(code, LoggedInUser);
        return Json(result);
    }

    public async Task<IActionResult> Active(string code)
    {
        var username = LoggedInUser;

        ResponseViewModel result = await _agentService.Active(code, LoggedInUser);
        return Json(result);
    }

    public async Task<IActionResult> Inactive(string code)
    {
        var username = LoggedInUser;

        ResponseViewModel result = await _agentService.Inactive(code, LoggedInUser);
        return Json(result);
    }
}