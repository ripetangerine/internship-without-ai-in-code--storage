
using Main.DTO.Regions;
namespace Main.Domain.Interfaces
{
    public interface IResourceInventoryClient
    {
        Task<IReadOnlyList<RegionsSummaryResponse>> GetRegionAsync(CancellationToken ct);
        Task<IReadOnlyList<SummaryResource>> GetResourceAsync(string regionId, CancellationToken ct);
    }
}
