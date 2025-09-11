using DotNetMemoryLab.Diagnostics.Abstractions;
using DotNetMemoryLab.Diagnostics.Allocation;
using DotNetMemoryLab.Diagnostics.Runtime;
using DotNetMemoryLab.Diagnostics.Services;
using DotNetMemoryLab.Domain.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetMemoryLab.Diagnostics;

/// <summary>
/// DI registration helpers for DotNetMemoryLab diagnostics.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds diagnostics services (allocations, counters, metrics provider) to the container.
    /// </summary>
    /// <param name="services"> The service collection. </param>
    /// <param name="configureOptions"> Optional options configuration. </param>
    /// <returns> The same service collection. </returns>
    public static IServiceCollection AddDotNetMemoryLabDiagnostics(
        this IServiceCollection services,
        Action<MetricsOptions>? configureOptions = null)
    {
        if (configureOptions is not null)
            _ = services.Configure(configureOptions);
        else
            _ = services.AddOptions<MetricsOptions>();

        _ = services.AddSingleton<GcMetricsAggregator>();

        _ = services.AddSingleton<IAllocator, MemoryAllocator>();
        _ = services.AddSingleton<IMemoryMetricsProvider, MemoryService>();

        // Hosted services:
        _ = services.AddHostedService<RuntimeCountersService>();
        _ = services.AddHostedService(sp => (MemoryService)sp.GetRequiredService<IMemoryMetricsProvider>());

        return services;
    }
}
