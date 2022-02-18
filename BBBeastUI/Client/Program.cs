using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Havit.Blazor.Components.Web;
using MetaMask.Blazor;
using NFT.Contract.Encoding;
using BBBeastUI.Services;
using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddOptions();
builder.Services.Configure<Web3Options>(builder.Configuration.GetSection("Web3"));

builder.Services.AddNFTContractEncoding();
builder.Services.AddMetaMaskBlazor();
builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<IWalletInteractionService, WalletInteractionService>();
builder.Services.AddScoped<INFTQueryService, NFTQueryService>();
builder.Services.AddScoped<IProvenanceHashService, ProvenanceHashService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
