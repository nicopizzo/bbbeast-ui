using Blazored.Toast.Services;
using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using MetaMask.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using nft.contract.encoding;
using nft.contract.query;
using nft.ui.Models;
using System.Numerics;

namespace nft.ui.Components
{
    public partial class WalletInteraction : ComponentBase
    {
        [Inject]
        IToastService _toastService { get; set; }

        [Inject]
        INFTEncoding _nftEncoding { get; set; }

        [Inject]
        INFTQuery _nftQuery { get; set; }

        [Inject]
        MetaMaskService _metaMaskService { get; set; }

        [Inject]
        Web3Options _web3Options { get; set; }

        private bool hasMetaMask;
        private string selectedAddress;
        private long chainId = -1;
        private int accountMinted = 0;
        private int mintCount = 0;
        private int mintLeft
        {
            get
            {
                return _web3Options.MaxMintCount - accountMinted;
            }
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
                    _toastService.ShowError("Invalid mint amount");
                    return;
                }

                BigInteger weiValue = BigInteger.Multiply(BigInteger.Parse(_web3Options.MintCost), mintCount);
                string data = _nftEncoding.GetMintFunctionEncoding(mintCount);

                var result = await _metaMaskService.SendTransaction(_web3Options.ContractAddress, weiValue, data[2..]);
                _toastService.ShowSuccess($"Transaction sent: {result}");
                await GetSelectedAddress();
            }
            catch (UserDeniedException)
            {
                _toastService.ShowError("User denied the transaction");
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unhandled exception: {ex}");
            }
        }

        private async Task ConnectWallet()
        {
            await _metaMaskService.ConnectMetaMask();
            await GetSelectedAddress();
            await GetSelectedNetwork();
        }

        private async Task GetSelectedAddress()
        {
            selectedAddress = await _metaMaskService.GetSelectedAddress();
            accountMinted = await _nftQuery.GetNFTCount(selectedAddress);
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
                _toastService.ShowWarning("Wallet signed out");
            }
            else
            {
                await GetSelectedAddress();
            }
            StateHasChanged();
        }

        private async Task ChainChanged((long, Chain) t)
        {
            _toastService.ShowWarning("Chain Changed");
            await GetSelectedNetwork();
            StateHasChanged();
        }

        private bool ValidateMint()
        {
            if(mintCount <= 0 || 
                mintCount > _web3Options.MaxMintCount ||
                mintCount > mintLeft ||
                accountMinted >= _web3Options.MaxMintCount)
            {
                return false;
            }

            return true;
        }
    }
}
