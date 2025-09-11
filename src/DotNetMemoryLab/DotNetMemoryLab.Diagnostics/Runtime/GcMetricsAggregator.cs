using DotNetMemoryLab.Domain.Enums;
using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Diagnostics.Runtime;

/// <summary>
/// Aggregates GC APIs and runtime counters into a cohesive view for snapshots.
/// </summary>
public sealed class GcMetricsAggregator
{
    // Fields updated by RuntimeCountersService (EventListener) callbacks.
    private long _gen0Size;
    private long _gen1Size;
    private long _gen2Size;
    private long _lohSize;
    private long _pohSize;
    private double _timeInGcPercent;
    private ulong _allocatedBytes;

    /// <summary>
    /// Updates a known counter by name with the provided numeric value.
    /// </summary>
    /// <param name="counterName"> Runtime counter name. </param>
    /// <param name="value"> Counter numeric value. </param>
    public void OnCounter(string counterName, double value)
    {
        switch (counterName)
        {
            case "gen-0-size":
            _gen0Size = (long)value;
            break;

            case "gen-1-size":
            _gen1Size = (long)value;
            break;

            case "gen-2-size":
            _gen2Size = (long)value;
            break;

            case "loh-size":
            _lohSize = (long)value;
            break;

            case "poh-size":
            _pohSize = (long)value;
            break;

            case "time-in-gc":
            // Some feeds provide ratio [0..1], others already [0..100].
            var v = value <= 1.0 ? value * 100.0 : value;
            _timeInGcPercent = Math.Clamp(v, 0, 100);
            break;

            case "allocated-bytes":
            _allocatedBytes = (ulong)Math.Max(0, value);
            break;
        }
    }

    /// <summary>
    /// Builds a <see cref="GcStats"/> and <see cref="HeapBreakdown"/> using GC APIs and last counters.
    /// </summary>
    /// <returns> Tuple with GC stats and heap breakdown. </returns>
    public (GcStats gc, HeapBreakdown heap) Build()
    {
        var info = GC.GetGCMemoryInfo();

        var gen0Collections = GC.CollectionCount(0);
        var gen1Collections = GC.CollectionCount(1);
        var gen2Collections = GC.CollectionCount(2);

        var gen0 = new GenerationStats
        {
            Generation = GenerationKind.Gen0,
            Size = new ByteSize(_gen0Size),
            Collections = gen0Collections
        };

        var gen1 = new GenerationStats
        {
            Generation = GenerationKind.Gen1,
            Size = new ByteSize(_gen1Size),
            Collections = gen1Collections
        };

        var gen2 = new GenerationStats
        {
            Generation = GenerationKind.Gen2,
            Size = new ByteSize(_gen2Size),
            Collections = gen2Collections
        };

        var loh = new SegmentStats { Kind = HeapKind.LOH, Size = new ByteSize(_lohSize) };
        var poh = new SegmentStats { Kind = HeapKind.POH, Size = new ByteSize(_pohSize) };

        var heap = new HeapBreakdown
        {
            Gen0 = gen0,
            Gen1 = gen1,
            Gen2 = gen2,
            Loh = loh,
            Poh = poh
        };

        var gc = new GcStats
        {
            Gen0Collections = gen0Collections,
            Gen1Collections = gen1Collections,
            Gen2Collections = gen2Collections,
            TotalAllocatedBytes = _allocatedBytes,
            TimeInGcPercent = new Percent(_timeInGcPercent),
            TotalCommitted = new ByteSize(info.TotalCommittedBytes),
            TotalHeap = new ByteSize(info.HeapSizeBytes),
            FragmentedBytes = new ByteSize(info.FragmentedBytes)
        };

        return (gc, heap);
    }
}
