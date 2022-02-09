using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Pages.Minting.Components
{
    public partial class TransactionModel : ComponentBase
    {
        [Inject]
        protected HttpClient _httpClient { get; set; }

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
                    var response = await _httpClient.GetAsync($"/api/nft/query/tx/{tx}", cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        transactionState = JsonSerializer.Deserialize<QueryResult<TransactionState>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })?.Data;
                        if (transactionState != TransactionState.Pending) break;
                    }
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
