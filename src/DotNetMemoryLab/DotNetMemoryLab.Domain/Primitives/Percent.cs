namespace DotNetMemoryLab.Domain.Primitives;

/// <summary>
/// Represents a percentage in range [0..100].
/// </summary>
public readonly record struct Percent
{
    /// <summary>
    /// Initializes a new instance of <see cref="Percent"/>.
    /// </summary>
    /// <param name="value"> Percentage value clamped to [0..100]. </param>
    public Percent(double value)
    {
        Value = Math.Clamp(value, 0d, 100d);
    }

    /// <summary>
    /// Gets the percentage value in [0..100].
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Gets a 0% value.
    /// </summary>
    public static Percent Zero { get; } = new(0d);

    /// <summary>
    /// Creates a percentage from a ratio in [0..1].
    /// </summary>
    /// <param name="ratio"> A number typically between 0 and 1. </param>
    public static Percent FromRatio(double ratio) => new(Math.Clamp(ratio * 100d, 0d, 100d));
}
