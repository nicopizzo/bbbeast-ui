using BBBeastUI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Havit.Blazor.Components.Web;
using BBBeastUI.Models;
using nft.contract;
using MetaMask.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var web3Options = new Web3Options();
builder.Configuration.Bind("Web3", web3Options);
builder.Services.AddSingleton(web3Options);

builder.Services.AddNFTContractInteraction(builder.Configuration);
builder.Services.AddMetaMaskBlazor();
builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
