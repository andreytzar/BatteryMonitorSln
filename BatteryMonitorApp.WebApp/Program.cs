using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using BatteryMonitorApp.Domain.DbContexts;
using BatteryMonitorApp.Domain.Repositories;
using Microsoft.OpenApi.Models;
using BatteryMonitorApp.WebApp.MapperConfigs;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Battery Monitor API",
        Description = "An ASP.NET Core Web API for managing battery"
    });
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton(_ =>
{
    MapperConfiguration mc = new(cfg => cfg.AddProfile<MapperConfigProfiles>());
    return mc.CreateMapper();
});

// Get Sql Connect String
var connectionString = builder.Configuration.GetConnectionString("SqlConnection") ??
    throw new InvalidOperationException("Connection string 'SqlConnection' not found.");

builder.Services.AddDbContext<IBatteryMonitorContext, BatteryMonitorContext>(c => c.UseSqlServer(connectionString,
    b => b.MigrationsAssembly("BatteryMonitorApp.WebApp")));

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddHealthChecks().AddSqlServer(
    connectionString, timeout: TimeSpan.FromSeconds(5));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<BatteryMonitorContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "swagger"));
app.UseMigrationsEndPoint();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapHealthChecks("/helth");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
public partial class Program { }