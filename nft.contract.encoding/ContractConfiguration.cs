using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using nft.contract.encoding;
using nft.contract.query;

namespace nft.contract
{
    public static class ContractConfiguration
    {
        public static IServiceCollection AddNFTContractInteraction(this IServiceCollection services, IConfiguration config)
        {
            var network = config.GetSection("Web3:Network").Value;
            var contractAddress = config.GetSection("Web3:ContractAddress").Value;
            
            if (network == "development")
            {
                services.AddSingleton<IClient, RpcClient>(f => new RpcClient(new Uri("HTTP://127.0.0.1:7545")));
                services.AddSingleton<IWeb3>(f => new Web3(f.GetRequiredService<IClient>()));
            }
            else
            {
                var projectId = config.GetSection("Web3:ProjectId").Value;
                services.AddSingleton<IWeb3>(f => new Web3($"https://{network}.infura.io/v3/{projectId}"));
            }
            
            services.AddSingleton<INFTEncoding, NFTEncoding>();
            services.AddSingleton<INFTQuery, NFTQuery>(f => new NFTQuery(f.GetRequiredService<IWeb3>(), contractAddress));
            return services;
        }
    }
}
