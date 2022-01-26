using Microsoft.Extensions.DependencyInjection;

namespace NFT.Contract.Encoding
{
    public static class ContractConfiguration
    {
        public static IServiceCollection AddNFTContractEncoding(this IServiceCollection services)
        {           
            services.AddSingleton<INFTEncoding, NFTEncoding>();
            return services;
        }
    }
}
