using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetMemoryLab.Diagnostics.Abstractions;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Presentation.ViewModels;

/// <summary>
/// Exposes commands to allocate and free memory for demonstration.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="ActionsViewModel"/>.
/// </remarks>
/// <param name="allocator"> Allocation service. </param>
public sealed partial class ActionsViewModel(IAllocator allocator) : ObservableObject
{
    [ObservableProperty] private long _allocationSizeBytes = ByteSize.FromKilobytes(64).Bytes;

    /// <summary>
    /// Sets the allocation size in bytes.
    /// </summary>
    /// <param name="bytes"> Number of bytes. </param>
    public void SetAllocationSize(long bytes) => AllocationSizeBytes = Math.Max(1, bytes);

    /// <summary>
    /// Allocates a buffer on SOH using the current size.
    /// </summary>
    [RelayCommand]
    public void AllocateSOH() => allocator.AllocateSOH(new ByteSize(AllocationSizeBytes));

    /// <summary>
    /// Allocates a buffer on LOH using the current size (coerced to LOH threshold if needed).
    /// </summary>
    [RelayCommand]
    public void AllocateLOH() => allocator.AllocateLOH(new ByteSize(AllocationSizeBytes));

    /// <summary>
    /// Allocates a pinned buffer which will be placed on POH.
    /// </summary>
    [RelayCommand]
    public void AllocatePOH() => allocator.AllocatePOH(new ByteSize(AllocationSizeBytes));

    /// <summary>
    /// Releases all allocated buffers and pinned handles, then forces a compacting GC.
    /// </summary>
    [RelayCommand]
    public void ReleaseAll() => allocator.ReleaseAll();

    /// <summary>
    /// Forces a compacting full GC.
    /// </summary>
    [RelayCommand]
    public void Collect() => allocator.CollectCompacting();

    /// <summary>
    /// Attempts to start a NoGCRegion using the current allocation size as a requested budget.
    /// </summary>
    [RelayCommand]
    public void StartNoGcRegion() => allocator.TryStartNoGcRegion(new ByteSize(AllocationSizeBytes));

    /// <summary>
    /// Ends the NoGCRegion if active.
    /// </summary>
    [RelayCommand]
    public void EndNoGcRegion() => allocator.EndNoGcRegion();
}
