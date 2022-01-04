using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace nft.contract
{
    public static class NFTContractConfiguration
    {
        public static IServiceCollection AddNFTContract(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<INFTService, NFTService>();
            return services;
        }
    }
}
