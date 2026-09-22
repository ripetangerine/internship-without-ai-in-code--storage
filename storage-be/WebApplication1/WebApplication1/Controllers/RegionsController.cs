using Microsoft.AspNetCore.Mvc;
using Main.DTO.Regions;
using Main.Domain.Interfaces;

namespace Main.Controllers
{

    [Route("/api/regions")]
    [ApiController]
    public class RegionsController(IResourceInventoryClient inventory) : ControllerBase
    {
        [HttpPost("summary")]
        public async Task<ActionResult<RegionsSummaryResponse[]>> GetSummary(CancellationToken ct)
        {
            var Regions = await inventory.GetRegionAsync(ct);
            // 리전별 동시조회

        }
    }
}
