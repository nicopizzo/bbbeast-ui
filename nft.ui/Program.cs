using Blazored.Toast;
using MetaMask.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using nft.ui;
using nft.ui.Services;

//var config = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json")
//    .Build();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMetaMaskBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IMintingService, MintingService>();

await builder.Build().RunAsync();
