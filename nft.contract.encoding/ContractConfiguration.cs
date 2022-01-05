using Microsoft.Extensions.DependencyInjection;
using nft.contract.encoding;
using nft.contract.query;

namespace nft.contract
{
    public static class ContractConfiguration
    {
        public static IServiceCollection AddNFTContractInteraction(this IServiceCollection services)
        {
            services.AddSingleton<INFTEncoding, NFTEncoding>();
            services.AddSingleton<INFTQuery, NFTQuery>();
            return services;
        }
    }
}
