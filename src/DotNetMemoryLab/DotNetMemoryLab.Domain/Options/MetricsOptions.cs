namespace DotNetMemoryLab.Domain.Options;

/// <summary>
/// Options to control sampling cadence and history expectations.
/// </summary>
public sealed record MetricsOptions
{
    /// <summary>
    /// Gets the fast poll interval for process-level metrics.
    /// </summary>
    public required TimeSpan ProcessPollInterval { get; init; }

    /// <summary>
    /// Gets the EventCounters interval (seconds) that diagnostics should try to match.
    /// </summary>
    public required TimeSpan RuntimeCountersInterval { get; init; }

    /// <summary>
    /// Gets the expected history length for sparkline buffers (number of points).
    /// </summary>
    public required int HistoryPoints { get; init; }

    /// <summary>
    /// Gets the minimum relative change for surfacing deltas in UI (0..1).
    /// </summary>
    public required double UiMinRelativeChange { get; init; }
}
