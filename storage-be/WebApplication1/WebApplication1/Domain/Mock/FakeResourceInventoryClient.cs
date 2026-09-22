namespace Main.Domain.Mock;
using Main.Domain.Interfaces;
using Main.DTO.Regions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FakeResourceInventoryClient : IResourceInventoryClient
{
    // 리전 이름은 임시. 실제 10개 리전으로 바꾸세요.
    private static readonly Region[] Regions = new[]
    {
        new Region("kr", "Korea"),
        new Region("jp", "Japan"),
        new Region("us", "USA"),
        new Region("cn", "China"),
        new Region("in", "India"),
        new Region("de", "Germany"),
        new Region("uk", "UK"),
        new Region("br", "Brazil"),
        new Region("au", "Australia"),
        new Region("sg", "Singapore"),
    };

    // 인터페이스에 맞게 메서드 이름을 단수형으로 변경
    public Task<IReadOnlyList<RegionsSummaryResponse>> GetRegionAsync(CancellationToken ct)
    {
        var list = Regions.Select(r => new RegionsSummaryResponse
        {
            Id = r.Id,
            Name = r.Name,
            // Status, ResourceCount, WarningCount, CriticalCount는 기본값(0 또는 enum의 0)으로 둡니다.
        }).ToList();

        return Task.FromResult<IReadOnlyList<RegionsSummaryResponse>>(list);
    }

    // 인터페이스에 맞게 메서드 이름과 반환 타입을 변경 (SummaryResource 리스트 반환)
    public Task<IReadOnlyList<SummaryResource>> GetResourceAsync(string regionId, CancellationToken ct)
    {
        // string.GetHashCode() 값 실행 달라짐 대처
        var seed = regionId.Aggregate(17, (h, c) => h * 31 + c);
        var rng = new Random(seed);

        var list = Enumerable.Range(1, 24).Select(i =>
        {
            var roll = rng.NextDouble();
            var cpu = roll switch
            {
                < 0.02 => rng.Next(90, 100),   // Critical
                < 0.10 => rng.Next(75, 90),    // Warning
                _ => rng.Next(10, 70),
            };
            var mem = rng.Next(20, 70);
            var disk = rng.Next(20, 70);
            var name = $"{regionId}-{(i % 6 == 0 ? "svc" : "vm")}-{i:00}";

            return new SummaryResource(
                name, name, i % 6 == 0 ? "Service" : "VM",
                StatusEvaluator.ForResource(cpu, mem, disk), cpu, mem, disk);
        }).ToList();

        return Task.FromResult<IReadOnlyList<SummaryResource>>(list);
    }
}