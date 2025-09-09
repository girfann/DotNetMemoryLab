using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Minimal thread info for stack visualization.
/// </summary>
public sealed record ThreadInfo
{
    /// <summary>
    /// Gets the thread identifier (OS or managed).
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Gets the estimated stack reserve for this thread.
    /// </summary>
    public required ByteSize StackReserve { get; init; }

    /// <summary>
    /// Gets the current managed call depth (proxy metric, optional).
    /// </summary>
    public int? ManagedCallDepth { get; init; }

    /// <summary>
    /// Gets a value indicating whether this is the UI thread.
    /// </summary>
    public bool IsUiThread { get; init; }
}
