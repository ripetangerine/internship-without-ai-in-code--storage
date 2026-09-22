using Main.Domain.Enum;

namespace Main.DTO.Regions
{
    public class RegionsSummaryResponse
    {
        public string Id { set; get; }
        public string Name { get; set; }
        public RegionHealthStatus Status { get; set; }
        public int ResourceCount { get; set; }
        public int WarningCount { get; set; }
        public int CriticalCount { get; set; }
        public
    }
}
