using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TMSWeb.Models;
//using TMSWeb.Models.Entities.HR;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;
using TMSWeb.Services;

var builder = WebApplication.CreateBuilder(args);

Progress = 0;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbTMSContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("servLOG"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Super", policy => policy.RequireClaim(ClaimTypes.Role, "Super"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CS", policy => policy.RequireClaim(ClaimTypes.Role, "CS"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Transport", policy => policy.RequireClaim(ClaimTypes.Role, "Transport"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Accounting", policy => policy.RequireClaim(ClaimTypes.Role, "Accounting"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
});

builder.Services.Scan(scan => scan
    .FromAssemblyOf<DashboardRepository>()
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
    .AsSelf()
    .WithScopedLifetime()
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "AVAOfficer";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);

    options.LoginPath = "/Authentication/SignIn/";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
    options.SlidingExpiration = true;
});

/*builder.Services.AddSingleton(sp =>
    new AllowanceService(
        ResolveJsonPath(sp, "Allowance:JsonPath", "Config/allowances.json", "ALLOWANCE_JSON_PATH")));

builder.Services.AddSingleton(sp =>
    new SsfService(
        ResolveJsonPath(sp, "Allowance:JsonPath", "Config/allowances.json", "ALLOWANCE_JSON_PATH")));*/

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = 4000;
    options.ValueLengthLimit = 1024 * 1024 * 1000;
});


var app = builder.Build();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
{
    SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") },
    SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US") },
    DefaultRequestCulture = new RequestCulture("en-US")
};
app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{code?}");
});

//RotativaConfiguration.Setup(app.Environment.ContentRootPath, "wwwroot/Rotativa");

app.Run();

partial class Program
{
    public static int Progress { get; set; }
    static string ResolveJsonPath(IServiceProvider sp, string configKey, string defaultRelative, string? envVar = null)
    {
        var cfg = sp.GetRequiredService<IConfiguration>();
        var env = sp.GetRequiredService<IHostEnvironment>();

        var raw = cfg[configKey]
                  ?? (envVar is null ? null : Environment.GetEnvironmentVariable(envVar))
                  ?? defaultRelative;

        // ตี path ให้เป็น absolute จาก ContentRoot ก่อน
        var path = Path.IsPathRooted(raw) ? raw : Path.Combine(env.ContentRootPath, raw);

        // ถ้าไม่เจอ ลองจาก AppContext.BaseDirectory อีกจุด (เช่น bin/<config>/)
        if (!File.Exists(path))
        {
            var alt = Path.IsPathRooted(raw) ? raw : Path.Combine(AppContext.BaseDirectory, raw);
            if (File.Exists(alt)) path = alt;
            else throw new FileNotFoundException($"JSON not found: {raw}. Tried: {path} | {alt}");
        }

        return path;
    }
}