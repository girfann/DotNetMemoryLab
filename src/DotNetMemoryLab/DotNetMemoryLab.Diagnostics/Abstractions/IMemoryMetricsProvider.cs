using DotNetMemoryLab.Domain.Models;

namespace DotNetMemoryLab.Diagnostics.Abstractions;

/// <summary>
/// Provides a live stream of memory snapshots for the application UI.
/// </summary>
public interface IMemoryMetricsProvider : IObservable<MemorySnapshot>
{
    /// <summary>
    /// Gets the most recent snapshot published by the provider.
    /// </summary>
    MemorySnapshot? Latest { get; }
}
