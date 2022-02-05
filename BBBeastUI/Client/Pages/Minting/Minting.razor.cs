using BBBeastUI.Pages.Minting.Components;
using BBBeastUI.Services;
using Microsoft.AspNetCore.Components;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Pages.Minting
{
    public partial class Minting : ComponentBase, IDisposable
    {
        [Inject]
        protected IWalletInteractionService _walletInteractionService { get; set; }

        [Inject]
        protected HttpClient _httpClient { get; set; }  

        private int? _TotalMinted;
        private WalletInteraction walletInteraction;

        protected override async Task OnInitializedAsync()
        {
            _walletInteractionService.RefreshRequested += Refresh;
            try
            {
                var result = await _httpClient.GetAsync("api/nft/query/minted");
                if (result.IsSuccessStatusCode)
                {
                    _TotalMinted = JsonSerializer.Deserialize<QueryResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })?.Count;
                }
            }
            catch { }
            await base.OnInitializedAsync();
        }

        private void Refresh()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            _walletInteractionService.RefreshRequested -= Refresh;
        }
    }
}
