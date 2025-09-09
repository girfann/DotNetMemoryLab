using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Process-level memory and thread counters.
/// </summary>
public sealed record ProcessMemory
{
    /// <summary>
    /// Gets the current working set size of the process.
    /// </summary>
    public required ByteSize WorkingSet { get; init; }

    /// <summary>
    /// Gets the current private bytes size of the process.
    /// </summary>
    public required ByteSize PrivateBytes { get; init; }

    /// <summary>
    /// Gets the current virtual memory size of the process.
    /// </summary>
    public required ByteSize VirtualMemory { get; init; }

    /// <summary>
    /// Gets the total number of OS threads in the process.
    /// </summary>
    public required int ThreadCount { get; init; }
}
