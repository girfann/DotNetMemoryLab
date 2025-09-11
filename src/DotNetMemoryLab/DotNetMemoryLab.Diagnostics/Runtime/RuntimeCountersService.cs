using System.Diagnostics.Tracing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetMemoryLab.Diagnostics.Runtime;

/// <summary>
/// Hosted service that owns an EventListener for System.Runtime counters
/// and feeds a shared <see cref="GcMetricsAggregator"/>.
/// </summary>
/// <param name="logger"> Logger. </param>
/// <param name="aggregator"> Aggregator to receive counter updates. </param>
public sealed class RuntimeCountersService(ILogger<RuntimeCountersService> logger, GcMetricsAggregator aggregator) : BackgroundService
{
    private EventListener? _listener;

    /// <inheritdoc />
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RuntimeCountersService starting.");

        _listener = new InternalListener(aggregator);

        var tcs = new TaskCompletionSource();

        _ = stoppingToken.Register(() =>
        {
            try
            {
                _listener?.Dispose();
            }
            finally
            {
                _listener = null;
                logger.LogInformation("RuntimeCountersService stopped.");
                _ = tcs.TrySetResult();
            }
        });

        return tcs.Task;
    }

    private sealed class InternalListener(GcMetricsAggregator aggregator) : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name == "System.Runtime")
            {
                EnableEvents(
                    eventSource,
                    EventLevel.Informational,
                    EventKeywords.All,
                    new Dictionary<string, string?> { ["EventCounterIntervalSec"] = "1" });
            }

            base.OnEventSourceCreated(eventSource);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData?.Payload is null)
                return;

            foreach (var payload in eventData.Payload)
            {
                if (payload is IDictionary<string, object> data &&
                    data.TryGetValue("Name", out var nameObj) &&
                    nameObj is string name)
                {
                    var value = 0d;

                    if (data.TryGetValue("Value", out var v) && v is double dv)
                        value = dv;
                    else if (data.TryGetValue("Mean", out var m) && m is double dm)
                        value = dm;
                    else if (data.TryGetValue("Increment", out var inc) && inc is double di)
                        value = di;

                    aggregator.OnCounter(name, value);
                }
            }
        }
    }
}
