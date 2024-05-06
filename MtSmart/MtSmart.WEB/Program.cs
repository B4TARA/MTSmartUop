using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MtSmart.BLL.Infrastructure.Configuration;
using MtSmart.BLL.Interfaces;
using MtSmart.BLL.Services;
using MtSmart.DAL;
using MtSmart.DAL.Interfaces;
using MtSmart.DAL.Repositories;
using MtSmart.WEB;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("WebApiDatabase");

// Чтение настроек из файла конфигурации
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Регистрация настроек в сервисном контейнере
var fileSettings = builder.Configuration.GetSection("FileSettings").Get<FileSettings>();
builder.Services.AddSingleton(fileSettings);

var imageSettings = builder.Configuration.GetSection("ImageSettings").Get<ImageSettings>();
builder.Services.AddSingleton(imageSettings);

// Добавление сервисов в контейнер.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/LogOut");
    });
builder.Services.AddAuthorization();

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
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

app.Run();
