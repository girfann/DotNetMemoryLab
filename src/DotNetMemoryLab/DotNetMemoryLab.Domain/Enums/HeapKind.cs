namespace DotNetMemoryLab.Domain.Enums;

/// <summary>
/// Logical heap segments in .NET managed memory.
/// </summary>
public enum HeapKind
{
    /// <summary>
    /// Small Object Heap.
    /// </summary>
    SOH = 0,

    /// <summary>
    /// Large Object Heap.
    /// </summary>
    LOH = 1,

    /// <summary>
    /// Pinned Object Heap.
    /// </summary>
    POH = 2
}
