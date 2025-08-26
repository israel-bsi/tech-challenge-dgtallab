using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Web;
using TechChallengeDgtallab.Web.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IDepartmentHandler, DepartmentHandler>();
builder.Services.AddScoped<ICollaboratorHandler, CollaboratorHandler>();

builder.Services
    .AddHttpClient(Configuration.HttpClientName, opt =>
    {
        opt.BaseAddress = new Uri(TechChallengeDgtallab.Core.Configuration.BackendUrl);
    });

await builder.Build().RunAsync();
