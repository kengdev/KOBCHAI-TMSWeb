using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMSWeb.Repositories;
using TMSWeb.Models.Entities.LOG;

namespace TMSWeb.Components;
public class EntryContainerJob : ViewComponent
{
    private readonly DropListRepository _dropListService;

    public EntryContainerJob(DropListRepository dropListService)
    {
        _dropListService = dropListService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        ViewData["ContainerSizeCode"] = from a in await _dropListService.GetContainerSize() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["AgentCode"] = from a in await _dropListService.GetAgent() select new SelectListItem { Value = a.key.ToString(), Text = a.label };
        ViewData["TruckTypegroupCode"] = from a in await _dropListService.GetTruckGroup() select new SelectListItem { Value = a.key.ToString(), Text = a.label };

        return View();
    }
}