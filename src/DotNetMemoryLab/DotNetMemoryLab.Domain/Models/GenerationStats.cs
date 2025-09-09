using DotNetMemoryLab.Domain.Enums;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Domain.Models;

/// <summary>
/// Stats for a single SOH generation.
/// </summary>
public sealed record GenerationStats
{
    /// <summary>
    /// Gets the generation kind.
    /// </summary>
    public required GenerationKind Generation { get; init; }

    /// <summary>
    /// Gets committed bytes of this generation.
    /// </summary>
    public required ByteSize Size { get; init; }

    /// <summary>
    /// Gets collections count for this generation.
    /// </summary>
    public required long Collections { get; init; }

    /// <summary>
    /// Gets optional recent delta for UI trend (may be zero).
    /// </summary>
    public ByteSize Delta { get; init; } = ByteSize.Zero;
}
