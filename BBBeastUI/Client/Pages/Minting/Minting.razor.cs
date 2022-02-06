using BBBeastUI.Models;
using BBBeastUI.Pages.Minting.Components;
using BBBeastUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Pages.Minting
{
    public partial class Minting : ComponentBase, IDisposable
    {
        [Inject]
        protected IWalletInteractionService _walletInteractionService { get; set; }

        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }

        [Inject]
        protected HttpClient _httpClient { get; set; }  

        private int? _TotalMinted;
        private ContractState contractState = ContractState.Public;
        private WalletInteraction walletInteraction;

        protected override async Task OnInitializedAsync()
        {
            _walletInteractionService.RefreshRequested += Refresh;
            try
            {
                var result = await _httpClient.GetAsync("api/nft/query/minted");
                if (result.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<MintPageResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    _TotalMinted = data?.TotalMinted;
                    contractState = data?.ContractState ?? ContractState.Public;
                }
            }
            catch { }
            await base.OnInitializedAsync();
        }

        private async Task Refresh()
        {
            StateHasChanged();
            await _jSRuntime.InvokeVoidAsync("Jazzicon.generateJazzicon", "#addressJazzicon", 30, walletInteraction.selectedAddress); 
        }

        public void Dispose()
        {
            _walletInteractionService.RefreshRequested -= Refresh;
        }
    }
}
