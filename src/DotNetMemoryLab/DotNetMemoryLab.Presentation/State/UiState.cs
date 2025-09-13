using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Presentation.State;

/// <summary>
/// Immutable UI-facing snapshot derived from <see cref="MemorySnapshot"/>.
/// Keeps a short history for charts.
/// </summary>
public sealed record UiState
{
    /// <summary>
    /// Gets the source domain snapshot.
    /// </summary>
    public required MemorySnapshot Snapshot { get; init; }

    /// <summary>
    /// Gets a rolling history (most recent first) of total managed heap sizes (bytes).
    /// </summary>
    public required IReadOnlyList<long> TotalManagedHistory { get; init; }

    /// <summary>
    /// Gets the ring capacity for history arrays.
    /// </summary>
    public required int HistoryCapacity { get; init; }

    /// <summary>
    /// Creates a new <see cref="UiState"/> with updated history by pushing the given value.
    /// </summary>
    /// <param name="totalManaged"> Current total managed size. </param>
    /// <returns> New UiState with updated history. </returns>
    public UiState WithPushedManaged(ByteSize totalManaged)
    {
        var history = TotalManagedHistory.Count == 0 ? [] : TotalManagedHistory.ToArray();

        var list = new List<long>(Math.Min(HistoryCapacity, (history.Length == 0 ? 0 : history.Length) + 1))
        {
            totalManaged.Bytes
        };

        if (history.Length > 0)
        {
            for (var i = 0; i < Math.Min(history.Length, HistoryCapacity - 1); i++)
                list.Add(history[i]);
        }

        return this with { TotalManagedHistory = list };
    }
}
