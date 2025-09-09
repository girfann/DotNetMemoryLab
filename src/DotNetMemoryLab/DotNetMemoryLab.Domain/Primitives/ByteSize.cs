namespace DotNetMemoryLab.Domain.Primitives;

/// <summary>
/// Represents a memory size in bytes with basic arithmetic and comparisons.
/// </summary>
public readonly record struct ByteSize
{
    /// <summary>
    /// Initializes a new instance of <see cref="ByteSize"/>.
    /// </summary>
    /// <param name="bytes">Number of bytes.</param>
    public ByteSize(long bytes)
    {
        Bytes = bytes;
    }

    /// <summary>
    /// Gets the number of bytes.
    /// </summary>
    public long Bytes { get; }

    /// <summary>
    /// Gets a zero-sized value.
    /// </summary>
    public static ByteSize Zero { get; } = new(0);

    /// <summary>
    /// Creates a value from bytes.
    /// </summary>
    public static ByteSize FromBytes(long bytes) => new(bytes);

    /// <summary>
    /// Creates a value from kilobytes (1024 bytes).
    /// </summary>
    public static ByteSize FromKilobytes(long kilobytes) => new(checked(kilobytes * 1_024L));

    /// <summary>
    /// Creates a value from megabytes (1024*1024 bytes).
    /// </summary>
    public static ByteSize FromMegabytes(long megabytes) => new(checked(megabytes * 1_024L * 1_024L));

    /// <summary>
    /// Adds two <see cref="ByteSize"/> values.
    /// </summary>
    public static ByteSize operator +(ByteSize a, ByteSize b) => new(checked(a.Bytes + b.Bytes));

    /// <summary>
    /// Subtracts two <see cref="ByteSize"/> values.
    /// </summary>
    public static ByteSize operator -(ByteSize a, ByteSize b) => new(checked(a.Bytes - b.Bytes));

    /// <summary>
    /// Compares two <see cref="ByteSize"/> values for less-than.
    /// </summary>
    public static bool operator <(ByteSize a, ByteSize b) => a.Bytes < b.Bytes;

    /// <summary>
    /// Compares two <see cref="ByteSize"/> values for greater-than.
    /// </summary>
    public static bool operator >(ByteSize a, ByteSize b) => a.Bytes > b.Bytes;

    /// <summary>
    /// Compares two <see cref="ByteSize"/> values for less-than-or-equal.
    /// </summary>
    public static bool operator <=(ByteSize a, ByteSize b) => a.Bytes <= b.Bytes;

    /// <summary>
    /// Compares two <see cref="ByteSize"/> values for greater-than-or-equal.
    /// </summary>
    public static bool operator >=(ByteSize a, ByteSize b) => a.Bytes >= b.Bytes;
}
