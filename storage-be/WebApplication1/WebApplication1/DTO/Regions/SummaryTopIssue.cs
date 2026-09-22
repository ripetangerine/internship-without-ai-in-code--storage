using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Main.DTO.Regions
{
    public record SummaryTopIssue(
        string ResourceId,
        string ResourceName,
        string Metric,
        int Value,
        HealthStatus Severity,
        DateTimeOffset Since);
}
