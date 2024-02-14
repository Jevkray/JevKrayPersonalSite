using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.PrivateServices.PrivateBackgroundServices;
using JevKrayPersonalSite.Workers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using JevKrayPersonalSite.Routing;

bool useNewVersion = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<Worker>(); // ������������ Worker ��� HostedService
builder.Services.AddScoped<GitHubLogger>();

builder.Services.AddDbContext<JevkSiteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

ViewEngineOptionsConfiguration.ConfigureViewEngineOptions(services: builder.Services, useNewVersion);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    RoutingConfiguration.ConfigureRoutes(endpoints, useNewVersion);
});
#pragma warning restore ASP0014


// ��������� Worker � ������� ������ (����� ��������� ���������� � github ����, �� ��� ���� �� ����������� github ������ - ���������.)
#pragma warning disable CS4014
Task.Run(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var worker = services.GetRequiredService<Worker>();
        await worker.StartAsync(default);
    }
});
#pragma warning restore CS4014 

app.Run();
