using Main.Domain.Enum;
using Main.DTO.Regions;

namespace Main.Domain.Mock
{
    // TODO: Status 관련 규칙 변경
    public class StatusEvaluator
    {
        private const int Warn = 75;
        private const int Crit = 90;

        public static RegionHealthStatus ForResource(int cpu, int mem, int disk)
        {
            var max = Math.Max(cpu, Math.Max(mem, disk));
            return max >= Crit ? RegionHealthStatus.Critical :
                    max >= Warn ? RegionHealthStatus.Warning :
                        RegionHealthStatus.Healthy;
        }

        public static RegionHealthStatus ForRegion(IReadOnlyCollection<SummaryResource> resources)
        {
            if (resources.Count == 0) return RegionHealthStatus.Unknown;
            if (resources.Any(r => r.Status == RegionHealthStatus.Critical)) return RegionHealthStatus.Critical;
            if (resources.Any(r => r.Status == RegionHealthStatus.Warning)) return RegionHealthStatus.Warning;
            return RegionHealthStatus.Healthy;
        }
    }
}
