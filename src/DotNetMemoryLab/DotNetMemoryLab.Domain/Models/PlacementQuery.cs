using DotNetMemoryLab.Domain.Enums;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Input for placement advisor to predict where a value would end up.
/// </summary>
public sealed record PlacementQuery
{
    /// <summary>
    /// Gets the input value kind (class, struct, array).
    /// </summary>
    public required ValueKind ValueKind { get; init; }

    /// <summary>
    /// Gets the size in bytes (arrays/objects; for structs an IL-size estimate).
    /// </summary>
    public required ByteSize Size { get; init; }

    /// <summary>
    /// Gets a value indicating whether the value/array is pinned.
    /// </summary>
    public bool IsPinned { get; init; }

    /// <summary>
    /// Gets a value indicating whether stack allocation is used (stackalloc/ref struct).
    /// </summary>
    public bool IsStackAlloc { get; init; }
}
