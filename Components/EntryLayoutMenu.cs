using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace TMSWeb.Components;
[Authorize]
public class EntryLayoutMenu : ViewComponent
{
    private string LoggedInUser => User.Identity.Name;
    private readonly DbTMSContext db;

    public EntryLayoutMenu(DbTMSContext context)
    {
        db = context;
    }
    public async Task<IViewComponentResult> InvokeAsync(string Top, string Mid, string Sub)
    {
        var ly = new LayoutRepository(db);
        var acc = new AccountRepository(db);
        var data = this.GetData();
        string username = LoggedInUser;

        var lv1 = await ly.GetPriviledgeLevel1ByGroup(data.group_code);
        List<UserPriviledgeModel> list_1 = new List<UserPriviledgeModel>();
        foreach (var item_1 in lv1)
        {
            var x = new UserPriviledgeModel();
            x.priviledge_code = item_1.Level01Code;
            x.priviledge_name = item_1.Level01Name;
            x.priviledge_icon = item_1.Level01Icon;
            x.priviledge_link = item_1.Level01Link;
            x.priviledge_active = item_1.Level01Active;
            var lv2 = await ly.GetPriviledgeLevel2ByGroup(data.group_code, item_1.Level01Code);
            List<UserPriviledgeModel> list_2 = new List<UserPriviledgeModel>();
            foreach (var item_2 in lv2)
            {
                var y = new UserPriviledgeModel();
                y.priviledge_code = item_2.Level02Code;
                y.priviledge_name = item_2.Level02Name;
                y.priviledge_link = item_2.Level02Link;
                y.priviledge_active = item_2.Level02Active;
                list_2.Add(y);
                var lv3 = await ly.GetPriviledgeLevel3ByGroup(data.group_code, item_2.Level02Code);
                List<UserPriviledgeModel> list_3 = new List<UserPriviledgeModel>();
                foreach (var item_3 in lv3)
                {
                    var j = new UserPriviledgeModel();
                    j.priviledge_code = item_3.Level03Code;
                    j.priviledge_name = item_3.Level03Name;
                    j.priviledge_link = item_3.Level03Link;
                    j.priviledge_active = item_3.Level03Active;
                    list_3.Add(j);
                }
                y.sub_level = list_3;
            }
            x.sub_level = list_2;
            list_1.Add(x);
        }

        ViewBag.Top = Top;
        ViewBag.Mid = Mid;
        ViewBag.Sub = Sub;
        ViewBag.Username = username;

        return View(list_1);
    }

    private ProfileViewModel GetData()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        //var theme = claimsIdentity.Claims.Where(w => w.Type == "webtheme").FirstOrDefault();
        var group = claimsIdentity.Claims.Where(w => w.Type == "group").FirstOrDefault();
        var model = new ProfileViewModel();
        model.username = User.Identity.Name;
        model.group_code = group.Value;

        return model;
    }
}