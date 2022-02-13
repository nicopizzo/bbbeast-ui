using BBBeast.UI.Shared.Models;

namespace BBBeast.UI.Shared.Interfaces
{
    public interface IProvenanceHashService
    {
        Task<HashDto> GetProvenanceHash();
    }
}
