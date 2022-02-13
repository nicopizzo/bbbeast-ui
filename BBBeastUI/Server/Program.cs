using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using BBBeastUI.Services;
using Havit.Blazor.Components.Web;
using MetaMask.Blazor;
using NFT.Contract.Encoding;
using NFT.Contract.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var web3Options = new Web3Options();
builder.Configuration.Bind("Web3", web3Options);
builder.Services.AddSingleton(web3Options);
builder.Services.AddNFTContractQuery(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddScoped<INFTQueryService, BBBeast.UI.Server.Services.NFTQueryService>();
builder.Services.AddScoped<IProvenanceHashService, BBBeast.UI.Server.Services.ProvHashService>();
builder.Services.AddNFTContractEncoding();

builder.Services.AddMetaMaskBlazor();
builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<IWalletInteractionService, WalletInteractionService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
