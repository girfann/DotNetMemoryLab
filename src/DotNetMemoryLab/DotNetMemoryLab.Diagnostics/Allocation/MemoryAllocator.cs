using System.Runtime.InteropServices;
using DotNetMemoryLab.Diagnostics.Abstractions;
using DotNetMemoryLab.Domain.Primitives;
using DotNetMemoryLab.Domain.Rules;

namespace DotNetMemoryLab.Diagnostics.Allocation;

/// <summary>
/// Produces controlled allocations for demonstration (SOH/LOH/POH).
/// </summary>
public sealed class MemoryAllocator : IAllocator, IDisposable
{
    private readonly List<byte[]?> _buffers = [];
    private readonly List<GCHandle> _pinned = [];

    private bool _noGcRegion;

    /// <inheritdoc />
    public void AllocateSOH(ByteSize size)
    {
        var bytes = (int)Math.Max(1, Math.Min(int.MaxValue, size.Bytes));

        _buffers.Add(new byte[bytes]);
    }

    /// <inheritdoc />
    public void AllocateLOH(ByteSize size)
    {
        var min = Math.Max(PlacementAdvisor.LohThreshold.Bytes, size.Bytes);
        var bytes = (int)Math.Min(int.MaxValue, min);

        _buffers.Add(new byte[bytes]);
    }

    /// <inheritdoc />
    public void AllocatePOH(ByteSize size)
    {
        var min = Math.Max(PlacementAdvisor.LohThreshold.Bytes, size.Bytes);
        var bytes = (int)Math.Min(int.MaxValue, min);
        var array = new byte[bytes];
        var handle = GCHandle.Alloc(array, GCHandleType.Pinned);

        _buffers.Add(array);
        _pinned.Add(handle);
    }

    /// <inheritdoc />
    public void ReleaseAll()
    {
        foreach (var handle in _pinned)
        {
            if (handle.IsAllocated)
                handle.Free();
        }

        _pinned.Clear();

        for (var i = 0; i < _buffers.Count; i++)
            _buffers[i] = null;

        _buffers.Clear();

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
    }

    /// <inheritdoc />
    public void CollectCompacting()
        => GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);

    /// <inheritdoc />
    public bool TryStartNoGcRegion(ByteSize totalSize)
    {
        if (_noGcRegion)
            return true;

        try
        {
            if (GC.TryStartNoGCRegion(totalSize.Bytes))
            {
                _noGcRegion = true;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc />
    public void EndNoGcRegion()
    {
        if (!_noGcRegion)
            return;

        try
        {
            GC.EndNoGCRegion();
        }
        finally
        {
            _noGcRegion = false;
        }
    }

    /// <summary>
    /// Disposes pinned handles and clears buffers.
    /// </summary>
    public void Dispose() => ReleaseAll();
}
