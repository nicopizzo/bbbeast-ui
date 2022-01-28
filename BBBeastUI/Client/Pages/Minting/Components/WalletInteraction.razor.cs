﻿using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using MetaMask.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using BBBeastUI.Models;
using System.Numerics;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using NFT.Contract.Encoding;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Pages.Minting.Components
{
    public partial class WalletInteraction : ComponentBase, IDisposable
    {
        [Inject] 
        protected IHxMessengerService _messenger { get; set; }

        [Inject]
        protected HttpClient _httpClient { get; set; }

        [Inject]
        protected INFTEncoding _encoder { get; set; }

        [Inject]
        protected MetaMaskService _metaMaskService { get; set; }

        [Inject]
        protected Web3Options _web3Options { get; set; }

        private bool hasMetaMask;
        private string selectedAddress;
        private long chainId = -1;
        private int? accountMinted = null;
        private int mintCount = 0;
        private int mintLeft
        {
            get
            {
                if (accountMinted == null || accountMinted == -1) return -1;
                return _web3Options.MaxMintCount - accountMinted.Value;
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
                var encodingResult = _encoder.GetMintFunctionEncoding(mintCount);
                var data = encodingResult.Result;

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
                (accountMinted != -1 && 
                (mintCount > mintLeft ||
                accountMinted >= _web3Options.MaxMintCount)))
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

        public void Dispose()
        {
            MetaMaskService.AccountChangedEvent -= AccountChanged;
            MetaMaskService.ChainChangedEvent -= ChainChanged;
        }
    }
}
