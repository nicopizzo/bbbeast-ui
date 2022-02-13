using BBBeast.UI.Shared.Interfaces;
using BBBeast.UI.Shared.Models;
using System.Text.Json;

namespace BBBeastUI.Services
{
    public class ProvenanceHashService : IProvenanceHashService
    {
        private readonly HttpClient _httpClient;

        public ProvenanceHashService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HashDto> GetProvenanceHash()
        {
            var result = await _httpClient.GetAsync("api/nft/hash");
            if (result.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<HashDto>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
    }
}
