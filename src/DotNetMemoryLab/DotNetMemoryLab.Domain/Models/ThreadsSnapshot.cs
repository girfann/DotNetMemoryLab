using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Snapshot of thread-related info for stack visualization.
/// </summary>
public sealed record ThreadsSnapshot
{
    /// <summary>
    /// Gets the total number of threads captured in the snapshot.
    /// </summary>
    public required int TotalThreads { get; init; }

    /// <summary>
    /// Gets the per-thread information list (may be sampled if too many).
    /// </summary>
    public required IReadOnlyList<ThreadInfo> Threads { get; init; }

    /// <summary>
    /// Gets the estimated total reserved stack size across listed threads.
    /// </summary>
    public ByteSize EstimatedTotalStackReserve
    {
        get
        {
            var total = 0L;
            foreach (var t in Threads)
                total += t.StackReserve.Bytes;
            return new ByteSize(total);
        }
    }
}
