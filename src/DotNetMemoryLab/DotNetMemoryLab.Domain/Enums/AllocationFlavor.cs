namespace DotNetMemoryLab.Domain.Enums;

/// <summary>
/// Predicted allocation flavor for placement visualization.
/// </summary>
public enum AllocationFlavor
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
    POH = 2,

    /// <summary>
    /// Stack allocation (temporary).
    /// </summary>
    Stack = 3
}
