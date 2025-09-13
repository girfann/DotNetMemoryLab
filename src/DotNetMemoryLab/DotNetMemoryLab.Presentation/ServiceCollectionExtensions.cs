using DotNetMemoryLab.Presentation.State;
using DotNetMemoryLab.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetMemoryLab.Presentation;

/// <summary>
/// DI registration helpers for DotNetMemoryLab presentation layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds presentation services and view models to the container.
    /// </summary>
    /// <param name="services"> Service collection. </param>
    /// <returns> The same service collection. </returns>
    public static IServiceCollection AddDotNetMemoryLabPresentation(this IServiceCollection services)
    {
        // App must register an IDispatcher implementation before or after this call.
        _ = services.AddSingleton<UiStore>();

        // ViewModels
        _ = services.AddTransient<ShellViewModel>();
        _ = services.AddTransient<HeapViewModel>();
        _ = services.AddTransient<StackViewModel>();
        _ = services.AddTransient<ActionsViewModel>();

        return services;
    }
}
