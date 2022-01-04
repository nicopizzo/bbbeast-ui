using Blazored.Toast;
using nft.contract.encoding;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using nft.ui;
using nft.ui.Models;
using MetaMask.Blazor;
using nft.ui.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var web3Options = new Web3Options();
builder.Configuration.Bind("Web3", web3Options);
builder.Services.AddSingleton(web3Options);

builder.Services.AddNFTContractEncoding();
builder.Services.AddMetaMaskBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IMintingService, MintingService>();
await builder.Build().RunAsync();
