using Domain.Entities;
using Infrastrucure;
using Infrastrucure.Persistence;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddIdentity<Users, AuthRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddResponseCompression()
    .AddHttpContextAccessor()
    .AddHttpClient()
    .AddMemoryCache()
    .AddControllersWithViews();
//.AddRazorRuntimeCompilation();
builder.Services.AddInfrastructureServices(builder.Configuration);
var app = builder.Build();
app.UseResponseCompression().UseHttpsRedirection();
var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
    }
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
                   name: "Admin",
                   pattern: "{area:exists}/{controller=Account}/{action=Login}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
//var scope = app.Services.CreateScope();
//var service = scope.ServiceProvider.GetService<IAdoUtility>();
//General.Configure(app.Services.GetRequiredService<IConfiguration>(), app.Services.GetRequiredService<IHttpContextAccessor>(), app.Services.GetRequiredService<IWebHostEnvironment>(), service);
//app.UseSwagger();
///app.UseSwaggerUI();
app.Run();

