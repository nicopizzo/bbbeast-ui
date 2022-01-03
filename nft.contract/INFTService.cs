using Nethereum.RPC.Eth.DTOs;
using nft.contract.contractdefinition;
using System.Numerics;

namespace nft.contract
{
    public interface INFTService
    {
        Task<string> MintRequestAsync(MintFunction mintFunction);
        Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null);
        Task<string> MintRequestAsync(BigInteger mintCount);

        Task<string> PrivateSaleMintRequestAsync(PrivateSaleMintFunction privateSaleMintFunction);
        Task<TransactionReceipt> PrivateSaleMintRequestAndWaitForReceiptAsync(PrivateSaleMintFunction privateSaleMintFunction, CancellationTokenSource cancellationToken = null);
        Task<string> PrivateSaleMintRequestAsync(BigInteger mintCount);
    }
}
