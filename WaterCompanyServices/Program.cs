using Microsoft.EntityFrameworkCore;
using WaterCompanyServices.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Consumer",
        pattern: "Consumer/{*action}",
        defaults: new {area = "Consumer", controller = "Consumer", action = "Index" });
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area=Home}/{controller=Home}/{action=Home}/{id?}");
});

/*
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Home}/{controller=Home}/{action=Home}/{id?}");
*/

app.Run();
