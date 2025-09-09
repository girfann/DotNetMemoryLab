using DotNetMemoryLab.Domain.Enums;
using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Rules;

/// <summary>
/// Predicts where a value will be placed by the runtime (SOH/LOH/POH/stack).
/// The logic is intentionally simplified to be stable and explanatory for UI.
/// </summary>
public static class PlacementAdvisor
{
    /// <summary>
    /// Gets the LOH threshold used for arrays/strings in explanatory UI (approx. 85 KB).
    /// </summary>
    public static readonly ByteSize LohThreshold = new(85_000);

    /// <summary>
    /// Provides a placement advice for the given query.
    /// </summary>
    /// <param name="query">Placement prediction input.</param>
    /// <returns>Placement advice with flavor and explanation.</returns>
    public static PlacementAdvice Advise(PlacementQuery query) => query.IsStackAlloc
            ? new PlacementAdvice
            {
                Flavor = AllocationFlavor.Stack,
                InitialGeneration = null,
                Explanation = "Temporary storage on the current method stack (lifetime until method returns)."
            }
            : query.IsPinned
            ? new PlacementAdvice
            {
                Flavor = AllocationFlavor.POH,
                InitialGeneration = null,
                Explanation = "Pinned object: goes to POH so it does not interfere with SOH compaction."
            }
            : query.ValueKind == ValueKind.Array && query.Size >= LohThreshold
            ? new PlacementAdvice
            {
                Flavor = AllocationFlavor.LOH,
                InitialGeneration = null,
                Explanation = "Large array (≥ ~85 KB): allocated on LOH (Large Object Heap)."
            }
            : new PlacementAdvice
            {
                Flavor = AllocationFlavor.SOH,
                InitialGeneration = GenerationKind.Gen0,
                Explanation = "Small object: allocated on SOH starting in Gen0; surviving objects may be promoted to Gen1/Gen2."
            };
}
