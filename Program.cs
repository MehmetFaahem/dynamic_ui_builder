using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UIBuilderApp;
using UIBuilderApp.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Register our services
builder.Services.AddSingleton<DesignService>();
builder.Services.AddScoped<LocalStorageService>();

await builder.Build().RunAsync();
