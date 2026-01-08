using Microsoft.EntityFrameworkCore;
using MinikodEnvanterWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml; // EPPlus'ý kullanabilmek için gerekli

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "NetCoreMvc.Auth";
        options.LoginPath = "/Giris/Login";
        options.AccessDeniedPath = "/Giris/Login";
    });

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

// EPPlus lisans baðlamýný ayarlama
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Session yönetimi için gerekli hizmetleri ekleme
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

// Authentication ve Session middleware'leri ekleme
app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // Session Middleware'i burada ekleyin

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Giris}/{action=Login}/{id?}");

app.Run();
