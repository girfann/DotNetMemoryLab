using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// GC-level counters and totals.
/// </summary>
public sealed record GcStats
{
    /// <summary>
    /// Gets the number of Gen0 collections since process start.
    /// </summary>
    public required long Gen0Collections { get; init; }

    /// <summary>
    /// Gets the number of Gen1 collections since process start.
    /// </summary>
    public required long Gen1Collections { get; init; }

    /// <summary>
    /// Gets the number of Gen2 collections since process start.
    /// </summary>
    public required long Gen2Collections { get; init; }

    /// <summary>
    /// Gets total bytes allocated since process start, if available.
    /// </summary>
    public required ulong TotalAllocatedBytes { get; init; }

    /// <summary>
    /// Gets percent of time spent in GC in the last measurement window.
    /// </summary>
    public required Percent TimeInGcPercent { get; init; }

    /// <summary>
    /// Gets total committed bytes of managed heap if provided by runtime.
    /// </summary>
    public required ByteSize TotalCommitted { get; init; }

    /// <summary>
    /// Gets total logical heap size (may equal committed depending on runtime).
    /// </summary>
    public required ByteSize TotalHeap { get; init; }

    /// <summary>
    /// Gets GC-reported fragmented bytes (may be zero if not supported).
    /// </summary>
    public required ByteSize FragmentedBytes { get; init; }
}
