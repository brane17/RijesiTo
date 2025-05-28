using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;

var builder = WebApplication.CreateBuilder(args);

//  EF Core with MySQL - Pomelo
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Servisi-  container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();
app.UseSession();
// Konfiguracija the HTTP request
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS vrijednost je  30 dana. Za promjenu - https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
