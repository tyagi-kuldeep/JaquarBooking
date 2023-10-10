using Application;
using Domain.Entities;
using Infrastrucure;
using Infrastrucure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApp.Areas.Admin.Mapping;
using WebApp.Models;
using Newtonsoft.Json.Serialization;
using Application.Common.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();
builder.Services.AddIdentity<Users, AuthRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddResponseCompression()
    .AddHttpContextAccessor()
    .AddHttpClient()
    .AddMemoryCache()
    .AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
      .AddNewtonsoftJson(options =>
      {
          options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver() { NamingStrategy = new CamelCaseNamingStrategy() };
      })
    .AddRazorRuntimeCompilation();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfileAdmin));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//https://localhost:7271/swagger/index.html
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Jaquar Booking System API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token into field. Note: DO NOT USE BEARER BEFORE JWT TOKEN.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



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
ModelsUtility.Configure(app.Services.GetRequiredService<IHttpContextAccessor>(), app.Services.GetRequiredService<IConfiguration>(), app.Services.GetRequiredService<IWebHostEnvironment>());
app.UseSwagger();
app.UseSwaggerUI();
app.Run();

