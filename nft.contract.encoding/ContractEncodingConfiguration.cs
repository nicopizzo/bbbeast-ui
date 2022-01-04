using Microsoft.Extensions.DependencyInjection;

namespace nft.contract.encoding
{
    public static class ContractEncodingConfiguration
    {
        public static IServiceCollection AddNFTContractEncoding(this IServiceCollection services)
        {
            services.AddSingleton<INFTEncoding, NFTEncoding>();
            return services;
        }
    }
}
