using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;

namespace NFT.Contract.Query
{
    public static class ContractConfiguration
    {
        public static IServiceCollection AddNFTContractQuery(this IServiceCollection services, IConfiguration config)
        {
            var networkAccess = config.GetSection("Web3:NetworkAccess").Value;            
            var contractAddress = config.GetSection("Web3:ContractAddress").Value;
            
            if (networkAccess == "RPC")
            {
                var rpcEndpoint = config.GetSection("Web3:RPCEndpoint").Value;
                services.AddSingleton<IClient, RpcClient>(f => new RpcClient(new Uri(rpcEndpoint)));
                services.AddSingleton<IWeb3>(f => new Web3(f.GetRequiredService<IClient>()));
            }
            else
            {
                var network = config.GetSection("Web3:Network").Value;
                var projectId = config.GetSection("Web3:ProjectId").Value;
                services.AddSingleton<IWeb3>(f => new Web3($"https://{network}.infura.io/v3/{projectId}"));
            }
            
            services.AddSingleton<INFTQuery, NFTQuery>(f => new NFTQuery(f.GetRequiredService<IWeb3>(), contractAddress));
            return services;
        }
    }
}
