using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.PrivateServices.PrivateBackgroundServices;
using JevKrayPersonalSite.Routing;
using JevKrayPersonalSite.Services;
using JevKrayPersonalSite.Services.ServiceInterfaces;
using JevKrayPersonalSite.Workers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<GitHubLogger>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<JevkSiteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

ViewEngineOptionsConfiguration.ConfigureViewEngineOptions(services: builder.Services);

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
    RoutingConfiguration.ConfigureRoutes(endpoints);
});
#pragma warning restore ASP0014


// Запускаем Worker в фоновом потоке (Чтобы обновлять информацию о github логе, но при этом не перегружать github сервер - запросами.)
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
