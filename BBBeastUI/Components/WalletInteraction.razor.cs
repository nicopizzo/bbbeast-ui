using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using MetaMask.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using nft.contract.encoding;
using nft.contract.query;
using BBBeastUI.Models;
using System.Numerics;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;

namespace BBBeastUI.Components
{
    public partial class WalletInteraction : ComponentBase
    {
        [Inject] 
        protected IHxMessengerService _messenger { get; set; }

        [Inject]
        protected INFTEncoding _nftEncoding { get; set; }

        [Inject]
        protected INFTQuery _nftQuery { get; set; }

        [Inject]
        protected MetaMaskService _metaMaskService { get; set; }

        [Inject]
        protected Web3Options _web3Options { get; set; }

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
                    _messenger.AddMessage(CreateMessage("Invalid mint amount", "Error"));
                    return;
                }

                BigInteger weiValue = BigInteger.Multiply(BigInteger.Parse(_web3Options.MintCost), mintCount);
                string data = _nftEncoding.GetMintFunctionEncoding(mintCount);

                var result = await _metaMaskService.SendTransaction(_web3Options.ContractAddress, weiValue, data[2..]);
                _messenger.AddMessage(CreateMessage($"Transaction sent: {result}", "Success"));
                await GetSelectedAddress();
            }
            catch (UserDeniedException)
            {
                _messenger.AddMessage(CreateMessage("User denied the transaction", "Error"));
            }
            catch (Exception ex)
            {
                _messenger.AddMessage(CreateMessage($"Unhandled exception: {ex}", "Error"));
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
                _messenger.AddMessage(CreateMessage("Wallet signed out", "Warning"));
            }
            else
            {
                await GetSelectedAddress();
            }
            StateHasChanged();
        }

        private async Task ChainChanged((long, Chain) t)
        {
            _messenger.AddMessage(CreateMessage("Chain changed", "Warning"));
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

        private MessengerMessage CreateMessage(string message, string type)
        {
            var icon = BootstrapIcon.Question;
            switch (type)
            {
                case "Error":
                    icon = BootstrapIcon.X;
                    break;
                case "Warning":
                    icon = BootstrapIcon.ExclamationTriangle;
                    break;
                case "Success":
                    icon = BootstrapIcon.Check;
                    break;
            }
            return new MessengerMessage() { AutohideDelay = 1500, Title = message, Icon = icon };
        }
    }
}
