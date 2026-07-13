using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using Microsoft.AspNetCore.Authentication;
using TMSWeb.Helpers;

namespace TMSWeb.Controllers;

public class AuthenticationController : Controller
{
    private readonly DbTMSContext db;
    private readonly LineLoginHelper _lineLoginHelper;
    //private string redirectUri = "https://1410-14-207-131-151.ngrok-free.app/authentication/lineauth";
    private string redirectUri = "https://avagloballogistics.com/authentication/lineauth";

    public AuthenticationController(DbTMSContext context)
    {
        db = context;
        _lineLoginHelper = new LineLoginHelper();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult SignIn() => (IActionResult)this.View();

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        var _log = new LogRepository(this.db);
        var _sign = new AuthenRepository(this.db);
        var _account = new AccountRepository(this.db);
        var result = await _sign.SignIn(model.Username, model.Password);
        if (ModelState.IsValid && result == true)
        {
            var data = _account.GetAccountByUser(model.Username);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, data.group_name),
                    new Claim("group", data.group_code),
                };

            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            _log.AddSystemLog("-", "Login successfuly.", model.Username);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            _log.AddSystemLog("-", "Login failed.", model.Username);
            return View(model);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> LogOff()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("SignIn", "Authentication");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login() => (IActionResult)this.View();

    public async Task<ActionResult> Line()
    {
        return Redirect(_lineLoginHelper.GetLoginUrl(redirectUri));
    }

    public async Task<ActionResult> LineAuth()
    {
        var code = HttpContext.Request.Query["code"];
        var token = await _lineLoginHelper.GetUserProfileByAuthToken(code, redirectUri);
        var _account = new AccountRepository(db);
        var _userdata = _account.GetAccountByLineId(token.UserId);
        if (_userdata != null)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _userdata.username),
                    new Claim(ClaimTypes.Role, _userdata.group_name),
                    new Claim("displayname", token.DisplayName),
                    new Claim("userid", token.UserId),
                    new Claim("avatar", token.PictureUrl),
                    new Claim("group", _userdata.group_code),
                };

            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            return Redirect("/home");
        }
        else
        {
            return Redirect("/error");
        }

    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Authentication");
    }

    
    public IActionResult AccessDenied()
    {
        return View();
    }
}
