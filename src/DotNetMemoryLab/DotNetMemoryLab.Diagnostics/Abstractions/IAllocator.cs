using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Diagnostics.Abstractions;

/// <summary>
/// Exposes simple allocation scenarios for demonstration purposes.
/// </summary>
public interface IAllocator
{
    /// <summary>
    /// Allocates a small-object-heap buffer of the specified size.
    /// </summary>
    /// <param name="size"> Requested size in bytes. </param>
    void AllocateSOH(ByteSize size);

    /// <summary>
    /// Allocates a large-object-heap buffer (size will be coerced to LOH threshold if needed).
    /// </summary>
    /// <param name="size"> Requested size in bytes. </param>
    void AllocateLOH(ByteSize size);

    /// <summary>
    /// Allocates a pinned buffer that will end up on POH.
    /// </summary>
    /// <param name="size"> Requested size in bytes. </param>
    void AllocatePOH(ByteSize size);

    /// <summary>
    /// Releases all allocated buffers and pinned handles.
    /// </summary>
    void ReleaseAll();

    /// <summary>
    /// Forces a compacting full GC.
    /// </summary>
    void CollectCompacting();

    /// <summary>
    /// Attempts to start a NoGCRegion with the requested size.
    /// </summary>
    /// <param name="totalSize"> The requested total size. </param>
    /// <returns> True if the region was started; otherwise false. </returns>
    bool TryStartNoGcRegion(ByteSize totalSize);

    /// <summary>
    /// Ends the NoGCRegion if it was active.
    /// </summary>
    void EndNoGcRegion();
}
