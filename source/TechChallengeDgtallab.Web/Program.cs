using Blazored.LocalStorage;
using MudBlazor.Services;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Web;
using TechChallengeDgtallab.Web.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IDepartmentHandler, DepartmentHandler>();
builder.Services.AddScoped<ICollaboratorHandler, CollaboratorHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
