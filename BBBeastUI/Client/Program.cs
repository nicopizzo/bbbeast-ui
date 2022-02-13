using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Havit.Blazor.Components.Web;
using MetaMask.Blazor;
using NFT.Contract.Encoding;
using BBBeastUI.Services;
using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var web3Options = new Web3Options();
builder.Configuration.Bind("Web3", web3Options);
builder.Services.AddSingleton(web3Options);

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
