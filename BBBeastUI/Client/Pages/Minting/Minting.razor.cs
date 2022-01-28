using Microsoft.AspNetCore.Components;
using NFT.Contract.Query;
using System.Text.Json;

namespace BBBeastUI.Pages.Minting
{
    public partial class Minting : ComponentBase
    {
        [Inject]
        protected HttpClient _httpClient { get; set; }

        private int? _TotalMinted;

        protected override async Task OnInitializedAsync()
        {
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
    }
}
