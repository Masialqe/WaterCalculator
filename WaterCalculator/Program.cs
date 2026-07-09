using Microsoft.AspNetCore.HttpOverrides;
using WaterCalculator.Common.Infrastructure.AccessControll;
using WaterCalculator.Common.Infrastructure.Cache;
using WaterCalculator.Common.Infrastructure.Limiters;
using WaterCalculator.Components;
using WaterCalculator.Components.Apartments.Access;
using WaterCalculator.Components.Shared.Toast;
using WaterCalculator.Database;
using WaterCalculator.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//TO MOVe
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<ApartmentUrlFactory>();
builder.Services.AddDatabase();
builder.Services.ConfigureCache();
builder.Services.AddApplicationFeatures();

builder.Services.ConfigureDevIdentity();
builder.Services.ConfigureRateLimiting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseRateLimiter();
app.UseIdentity();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
