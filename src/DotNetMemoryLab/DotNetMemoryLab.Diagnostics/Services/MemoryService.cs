using DotNetMemoryLab.Diagnostics.Abstractions;
using DotNetMemoryLab.Diagnostics.Process;
using DotNetMemoryLab.Diagnostics.Runtime;
using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNetMemoryLab.Diagnostics.Services;

/// <summary>
/// Background service that periodically samples process/GC metrics and publishes <see cref="MemorySnapshot"/>.
/// </summary>
/// <param name="logger"> Logger. </param>
/// <param name="aggregator"> GC metrics aggregator. </param>
/// <param name="options"> Metrics options. </param>
public sealed class MemoryService(
    ILogger<MemoryService> logger,
    GcMetricsAggregator aggregator,
    IOptions<MetricsOptions> options) : BackgroundService, IMemoryMetricsProvider
{
    private readonly MetricsOptions _options = options.Value;
    private readonly List<IObserver<MemorySnapshot>> _observers = [];

    /// <inheritdoc />
    public MemorySnapshot? Latest { get; private set; }

    /// <inheritdoc />
    public IDisposable Subscribe(IObserver<MemorySnapshot> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Add(observer);

        if (Latest is not null)
            observer.OnNext(Latest);

        return new Unsubscriber(_observers, observer);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MemoryService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (gc, heap) = aggregator.Build();
                var proc = ProcessMemoryReader.Read();

                var threads = new ThreadsSnapshot
                {
                    TotalThreads = proc.ThreadCount,
                    Threads = []
                };

                var snapshot = new MemorySnapshot
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Process = proc,
                    Gc = gc,
                    Heap = heap,
                    Threads = threads
                };

                Latest = snapshot;

                foreach (var obs in _observers.ToArray())
                    obs.OnNext(snapshot);

                await Task.Delay(_options.ProcessPollInterval, stoppingToken);
            }
        }
        catch (TaskCanceledException) { /* graceful stop */ }
        finally
        {
            logger.LogInformation("MemoryService stopped.");
        }
    }

    private sealed class Unsubscriber(List<IObserver<MemorySnapshot>> list, IObserver<MemorySnapshot> observer) : IDisposable
    {
        public void Dispose() => list.Remove(observer);
    }
}
