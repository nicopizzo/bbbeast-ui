using BBBeast.UI.Shared.Interfaces;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using NFT.Contract.Query;

namespace BBBeastUI.Pages.Minting.Components
{
    public partial class TransactionModel : ComponentBase
    {
        [Inject]
        protected INFTQueryService _NFTQueryService { get; set; }

        private HxModal model;
        private string transactionId;
        private TransactionState? transactionState = TransactionState.Pending;
        private CancellationTokenSource tokenSource;

        public async Task ShowModel(string tx)
        {
            tokenSource = new CancellationTokenSource();
            transactionId = tx;
            StateHasChanged();
            await model.ShowAsync();
            await WatchTransaction(tx, tokenSource.Token);
            StateHasChanged();
        }

        private async Task CloseModel()
        {
            tokenSource.Cancel();
            await model.HideAsync();
            transactionId = null;
            tokenSource.Dispose();
        }

        private async Task WatchTransaction(string tx, CancellationToken cancellationToken)
        {
            int count = 0;
            await Task.Delay(1000);
            try
            {
                while(transactionState == TransactionState.Pending)
                {
                    if (cancellationToken.IsCancellationRequested || count == 10) break;
                    var result = await _NFTQueryService.GetTransactionState(tx, cancellationToken);
                    transactionState = result.Data;
                    if (transactionState != TransactionState.Pending) break;
                    count++;
                    await Task.Delay(10000);
                }              
            }
            catch
            {
            }          
        }
    }
}
