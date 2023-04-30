using BusStation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BusStation.Data;
using BusStation.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<CourseDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDB")));

builder.Services.AddDbContext<BusStationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDB")));

builder.Services.AddIdentity<BusStationUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false).
        AddEntityFrameworkStores<BusStationContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
    opt.LoginPath        = new PathString("/Identity/Account/Login");
});

//Password settings
builder.Services.Configure<IdentityOptions>(options =>
{   
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
