using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using MetaMask.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using BBBeastUI.Models;
using System.Numerics;
using NFT.Contract.Encoding;
using NFT.Contract.Query;
using System.Text.Json;
using BBBeastUI.Services;

namespace BBBeastUI.Pages.Minting.Components
{
    public partial class WalletInteraction : ComponentBase, IDisposable
    {
        [Inject]
        protected IWalletInteractionService _walletInteractionService { get; set; }

        [Inject] 
        protected IToastService _messenger { get; set; }

        [Inject]
        protected HttpClient _httpClient { get; set; }

        [Inject]
        protected INFTEncoding _encoder { get; set; }

        [Inject]
        protected MetaMaskService _metaMaskService { get; set; }

        [Inject]
        protected Web3Options _web3Options { get; set; }

        
        private bool hasMetaMask;
        private long chainId = -1;   
        private int mintCount = 0;
        public string selectedAddress;
        public int? accountMinted = null;
        private int mintLeft
        {
            get
            {
                if (accountMinted == null || accountMinted == -1) return -1;
                return _web3Options.MaxMintCount - accountMinted.Value;
            }
        }

        public async Task ConnectWallet()
        {
            await _metaMaskService.ConnectMetaMask();
            await GetSelectedAddress();
            await GetSelectedNetwork();
        }

        protected async override Task OnInitializedAsync()
        {
            MetaMaskService.AccountChangedEvent += AccountChanged;
            MetaMaskService.ChainChangedEvent += ChainChanged;

            hasMetaMask = await _metaMaskService.HasMetaMask();
            if (hasMetaMask) await _metaMaskService.ListenToEvents();

            bool isSiteConnected = await _metaMaskService.IsSiteConnected();
            if (isSiteConnected)
            {
                await GetSelectedAddress();
                await GetSelectedNetwork();
            }
        }

        private async Task Mint()
        {
            try
            {
                if (!ValidateMint())
                {
                    _messenger.PublishMessage("Invalid mint amount", ToastType.Failure);
                    return;
                }

                BigInteger weiValue = BigInteger.Multiply(BigInteger.Parse(_web3Options.MintCost), mintCount);
                var encodingResult = _encoder.GetMintFunctionEncoding(mintCount);
                var data = encodingResult.Result;

                var result = await _metaMaskService.SendTransaction(_web3Options.ContractAddress, weiValue, data[2..]);
                _messenger.PublishMessage($"Transaction sent: {result}", ToastType.Success);
                await GetSelectedAddress();
            }
            catch (UserDeniedException)
            {
                _messenger.PublishMessage("User denied the transaction", ToastType.Failure);
            }
            catch (Exception ex)
            {
                _messenger.PublishMessage($"Unhandled exception: {ex}", ToastType.Failure);
            }
        }

        private async Task GetSelectedAddress()
        {
            selectedAddress = await _metaMaskService.GetSelectedAddress();
            try
            {
                var result = await _httpClient.GetAsync($"/api/nft/query/count/{selectedAddress}");
                if (result.IsSuccessStatusCode)
                {
                    accountMinted = JsonSerializer.Deserialize<QueryResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })?.Count;
                }
                else
                {
                    accountMinted = -1;
                }
            }
            catch
            {
                accountMinted = -1;
            }
            StateHasChanged();
            _walletInteractionService.CallRequestRefresh();
        }

        private async Task GetSelectedNetwork()
        {
            var chainInfo = await _metaMaskService.GetSelectedChain();
            chainId = chainInfo.chainId;
        }

        private async Task AccountChanged(string t)
        {
            if (string.IsNullOrEmpty(t))
            {
                selectedAddress = null;
                _messenger.PublishMessage("Wallet signed out", ToastType.Warning);
            }
            else
            {
                await GetSelectedAddress();
            }
            StateHasChanged();
            _walletInteractionService.CallRequestRefresh();
        }

        private async Task ChainChanged((long, Chain) t)
        {
            _messenger.PublishMessage("Chain changed", ToastType.Warning);
            await GetSelectedNetwork();
            StateHasChanged();
            _walletInteractionService.CallRequestRefresh();
        }

        private void AddCount()
        {
            if (mintCount + 1 <= mintLeft) mintCount++;
            StateHasChanged();
        }

        private void MinusCount()
        {
            if (mintCount - 1 >= 0) mintCount--;
            StateHasChanged();
        }

        private bool ValidateMint()
        {
            if(mintCount <= 0 || 
                mintCount > _web3Options.MaxMintCount ||
                (accountMinted != -1 && 
                (mintCount > mintLeft ||
                accountMinted >= _web3Options.MaxMintCount)))
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            MetaMaskService.AccountChangedEvent -= AccountChanged;
            MetaMaskService.ChainChangedEvent -= ChainChanged;
        }
    }
}
