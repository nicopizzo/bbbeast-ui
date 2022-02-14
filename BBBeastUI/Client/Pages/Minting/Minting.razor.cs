using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using BBBeastUI.Pages.Minting.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NFT.Contract.Query;

namespace BBBeastUI.Pages.Minting
{
    public partial class Minting : ComponentBase, IDisposable
    {
        [Inject]
        protected Web3Options _web3Options { get; set; }

        [Inject]
        protected IWalletInteractionService _walletInteractionService { get; set; }

        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }

        [Inject]
        protected INFTQueryService _nftQueryService { get; set; }  

        [Inject]
        protected PersistentComponentState applicationState { get; set; }

        private MintPageResult _MintResult;
        private WalletInteraction walletInteraction;
        private PersistingComponentStateSubscription _subscription;

        protected override async Task OnInitializedAsync()
        {
            _subscription = applicationState.RegisterOnPersisting(PersistMintData);            
            try
            {
                _walletInteractionService.RefreshRequested += Refresh;
                if (applicationState.TryTakeFromJson<MintPageResult>("fetchData", out var stored))
                {
                    _MintResult = stored;
                }
                else
                {
                    _MintResult = await _nftQueryService.GetTotalMinted();
                }    
            }
            catch
            {
            }
        }

        private Task PersistMintData()
        {
            applicationState.PersistAsJson("fetchData", _MintResult);
            return Task.CompletedTask;
        }

        private async Task Refresh()
        {
            StateHasChanged();
            await _jSRuntime.InvokeVoidAsync("Jazzicon.generateJazzicon", "#addressJazzicon", 30, walletInteraction.selectedAddress); 
        }

        public void Dispose()
        {
            _walletInteractionService.RefreshRequested -= Refresh;
            _subscription.Dispose();
        }
    }
}
