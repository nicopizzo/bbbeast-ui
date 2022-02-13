using BBBeast.UI.Shared.Interfaces;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Services
{
    public class NFTQueryService : INFTQueryService
    {
        private readonly HttpClient _HttpClient;

        public NFTQueryService(HttpClient httpClient)
        {
            _HttpClient = httpClient;
        }

        public async Task<QueryResult<int>> GetMintedAmount(string address, CancellationToken cancellationToken = default)
        {
            var queryResult = new QueryResult<int>() { IsSuccess = false, Data = -1 };
            var result = await _HttpClient.GetAsync($"/api/nft/query/count/{address}", cancellationToken).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                queryResult = JsonSerializer.Deserialize<QueryResult<int>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return queryResult;
        }

        public async Task<MintPageResult> GetTotalMinted(CancellationToken cancellationToken = default)
        {
            var result = await _HttpClient.GetAsync("api/nft/query/minted", cancellationToken).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<MintPageResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return data;
            }
            return null;
        }

        public async Task<QueryResult<TransactionState>> GetTransactionState(string tx, CancellationToken cancellationToken = default)
        {
            var result = new QueryResult<TransactionState> { IsSuccess = false, Data = TransactionState.Pending };
            var response = await _HttpClient.GetAsync($"/api/nft/query/tx/{tx}", cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result = JsonSerializer.Deserialize<QueryResult<TransactionState>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return result;
        }
    }
}
