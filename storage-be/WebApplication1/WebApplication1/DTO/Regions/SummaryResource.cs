using Main.Domain.Enum;

namespace Main.DTO.Regions
{
    public record SummaryResource(
        string Id,
        string Name,
        string Type,
        RegionHealthStatus Status,
        int Cpu,
        int Memory,
        int Disk,
        DateTimeOffset? StatusSince)
    {
        private string name1;
        private string name2;
        private string v;
        private RegionHealthStatus regionHealthStatus;
        private int mem;

        public SummaryResource(string name1, string name2, string v, RegionHealthStatus regionHealthStatus, int cpu, int mem, int disk)
        {
            this.name1 = name1;
            this.name2 = name2;
            this.v = v;
            this.regionHealthStatus = regionHealthStatus;
            Cpu = cpu;
            this.mem = mem;
            Disk = disk;
        }
    }
}
