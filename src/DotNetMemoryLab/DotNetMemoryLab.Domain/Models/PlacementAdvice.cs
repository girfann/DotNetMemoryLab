using DotNetMemoryLab.Domain.Enums;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Advisor result with explanation and predicted heap segment.
/// </summary>
public sealed record PlacementAdvice
{
    /// <summary>
    /// Gets the predicted allocation flavor (SOH/LOH/POH/Stackalloc).
    /// </summary>
    public required AllocationFlavor Flavor { get; init; }

    /// <summary>
    /// Gets the predicted initial generation for SOH, if applicable.
    /// </summary>
    public GenerationKind? InitialGeneration { get; init; }

    /// <summary>
    /// Gets the human-readable explanation.
    /// </summary>
    public required string Explanation { get; init; }
}
