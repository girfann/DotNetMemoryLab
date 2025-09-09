using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Logical split of the managed heap: SOH (by generations), LOH, POH.
/// </summary>
public sealed record HeapBreakdown
{
    /// <summary>
    /// Gets stats for Generation 0 in SOH.
    /// </summary>
    public required GenerationStats Gen0 { get; init; }

    /// <summary>
    /// Gets stats for Generation 1 in SOH.
    /// </summary>
    public required GenerationStats Gen1 { get; init; }

    /// <summary>
    /// Gets stats for Generation 2 in SOH.
    /// </summary>
    public required GenerationStats Gen2 { get; init; }

    /// <summary>
    /// Gets stats for Large Object Heap.
    /// </summary>
    public required SegmentStats Loh { get; init; }

    /// <summary>
    /// Gets stats for Pinned Object Heap.
    /// </summary>
    public required SegmentStats Poh { get; init; }

    /// <summary>
    /// Gets the total size of SOH (Gen0+Gen1+Gen2).
    /// </summary>
    public ByteSize SohTotal => Gen0.Size + Gen1.Size + Gen2.Size;

    /// <summary>
    /// Gets total committed size across SOH, LOH and POH.
    /// </summary>
    public ByteSize TotalCommitted => SohTotal + Loh.Size + Poh.Size;

    /// <summary>
    /// Calculates the share of SOH relative to total, as percentage.
    /// </summary>
    public Percent SohPercentOfTotal =>
        TotalCommitted.Bytes == 0 ? Percent.Zero : Percent.FromRatio(SohTotal.Bytes / (double)TotalCommitted.Bytes);

    /// <summary>
    /// Calculates the share of LOH relative to total, as percentage.
    /// </summary>
    public Percent LohPercentOfTotal =>
        TotalCommitted.Bytes == 0 ? Percent.Zero : Percent.FromRatio(Loh.Size.Bytes / (double)TotalCommitted.Bytes);

    /// <summary>
    /// Calculates the share of POH relative to total, as percentage.
    /// </summary>
    public Percent PohPercentOfTotal =>
        TotalCommitted.Bytes == 0 ? Percent.Zero : Percent.FromRatio(Poh.Size.Bytes / (double)TotalCommitted.Bytes);
}
