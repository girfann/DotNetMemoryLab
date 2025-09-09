using DotNetMemoryLab.Domain.Enums;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Stats for a non-generational heap segment (LOH or POH).
/// </summary>
public sealed record SegmentStats
{
    /// <summary>
    /// Gets the heap kind (LOH or POH).
    /// </summary>
    public required HeapKind Kind { get; init; }

    /// <summary>
    /// Gets committed bytes for this segment.
    /// </summary>
    public required ByteSize Size { get; init; }

    /// <summary>
    /// Gets optional recent delta for UI trend (may be zero).
    /// </summary>
    public ByteSize Delta { get; init; } = ByteSize.Zero;
}
