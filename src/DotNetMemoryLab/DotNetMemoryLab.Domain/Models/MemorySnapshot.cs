using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Point-in-time snapshot of process and managed memory state.
/// </summary>
public sealed record MemorySnapshot
{
    /// <summary>
    /// Gets the capture timestamp.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// Gets process-level metrics (working set/private/virtual/thread count).
    /// </summary>
    public required ProcessMemory Process { get; init; }

    /// <summary>
    /// Gets garbage collector high-level stats.
    /// </summary>
    public required GcStats Gc { get; init; }

    /// <summary>
    /// Gets the detailed heap breakdown by generations and segments.
    /// </summary>
    public required HeapBreakdown Heap { get; init; }

    /// <summary>
    /// Gets the threads snapshot used for stack visualization.
    /// </summary>
    public required ThreadsSnapshot Threads { get; init; }

    /// <summary>
    /// Gets the total managed committed size derived from heap breakdown.
    /// </summary>
    public ByteSize TotalManagedCommitted => Heap.TotalCommitted;

    /// <summary>
    /// Gets the GC-reported fragmented bytes for convenience.
    /// </summary>
    public ByteSize FragmentedBytes => Gc.FragmentedBytes;
}
