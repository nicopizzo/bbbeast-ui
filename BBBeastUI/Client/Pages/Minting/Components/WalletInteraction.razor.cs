﻿using MetaMask.Blazor;
using MetaMask.Blazor.Enums;
using MetaMask.Blazor.Exceptions;
using Microsoft.AspNetCore.Components;
using System.Numerics;
using NFT.Contract.Encoding;
using NFT.Contract.Query;
using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using Microsoft.Extensions.Options;

namespace BBBeastUI.Pages.Minting.Components
{
    public partial class WalletInteraction : ComponentBase, IDisposable
    {
        [Inject]
        protected IWalletInteractionService _walletInteractionService { get; set; }

        [Inject] 
        protected IToastService _messenger { get; set; }

        [Inject]
        protected INFTQueryService _queryService { get; set; }

        [Inject]
        protected INFTEncoding _encoder { get; set; }

        [Inject]
        protected MetaMaskService _metaMaskService { get; set; }

        [Inject]
        protected IOptions<Web3Options> _web3Options { get; set; }

        [Parameter]
        public ContractState _contractState { get; set; }

        private TransactionModel _transactionModel;

        
        private bool hasMetaMask;
        private long chainId = -1;   
        private int mintCount = 0;
        public string selectedAddress { get; private set; }
        public string selectedAddressENS { get; private set; }
        public int? accountMinted { get; private set; } = null;
        public bool isLoading { get; private set; }
        private int mintLeft
        {
            get
            {
                if (accountMinted == null || accountMinted == -1) return -1;
                return _web3Options.Value.MaxMintCount - accountMinted.Value;
            }
        }

        public async Task ConnectWallet()
        {
            await _metaMaskService.ConnectMetaMask();
            await GetSelectedAddress();
            await GetSelectedNetwork();
        }

        protected override void OnInitialized()
        {
            MetaMaskService.AccountChangedEvent += AccountChanged;
            MetaMaskService.ChainChangedEvent += ChainChanged;
            isLoading = true;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                hasMetaMask = await _metaMaskService.HasMetaMask();
                if (hasMetaMask) await _metaMaskService.ListenToEvents();

                bool isSiteConnected = await _metaMaskService.IsSiteConnected();
                if (isSiteConnected)
                {
                    await GetSelectedAddress();
                    await GetSelectedNetwork();
                }
                isLoading = false;
                _walletInteractionService.CallRequestRefresh();
            }
        }

        private async Task Mint()
        {
            try
            {
                if (!ValidateMint())
                {
                    _messenger.PublishMessage("Invalid mint amount/inactive state", ToastType.Failure);
                    return;
                }

                BigInteger weiValue = BigInteger.Multiply(BigInteger.Parse(_web3Options.Value.PublicMintCost), mintCount);
                EncodingResult encodingResult = _encoder.GetMintFunctionEncoding(mintCount);

                if (_contractState == ContractState.Private)
                {
                    weiValue = BigInteger.Multiply(BigInteger.Parse(_web3Options.Value.PrivateMintCost), mintCount);
                    encodingResult = _encoder.GetPrivateSaleMintFunctionEncoding(mintCount);
                }

                var data = encodingResult.Result;

                var tx = await _metaMaskService.SendTransaction(_web3Options.Value.ContractAddress, weiValue, data[2..]);
                await _transactionModel.ShowModel(tx);
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
            var mintAmountTask = UpdateMintedAmount();
            var ensTask = UpdateENS();
            await Task.WhenAll(mintAmountTask, ensTask);
        }

        private async Task GetSelectedNetwork()
        {
            var chainInfo = await _metaMaskService.GetSelectedChain();
            chainId = chainInfo.chainId;
            StateHasChanged();
        }

        private async Task UpdateMintedAmount()
        {
            try
            {
                var result = await _queryService.GetMintedAmount(selectedAddress);
                accountMinted = result.Data;
            }
            catch
            {
                accountMinted = -1;
            }
            StateHasChanged();
            _walletInteractionService.CallRequestRefresh();
        }

        private async Task UpdateENS()
        {
            try
            {
                var result = await _queryService.ENSReverseLookup(selectedAddress);
                selectedAddressENS = result.Data;
            }
            catch
            {
                accountMinted = null;
            }
            StateHasChanged();
            _walletInteractionService.CallRequestRefresh();
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
                accountMinted = null;
                StateHasChanged();
                _walletInteractionService.CallRequestRefresh();
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
            if(_contractState == ContractState.NotLive ||
                mintCount <= 0 || 
                mintCount > _web3Options.Value.MaxMintCount ||
                (accountMinted != -1 && 
                (mintCount > mintLeft ||
                accountMinted >= _web3Options.Value.MaxMintCount)))
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
