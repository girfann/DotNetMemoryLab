namespace DotNetMemoryLab.Presentation.Abstractions;

/// <summary>
/// Abstraction for marshalling actions to the UI thread.
/// The app layer must provide an implementation (e.g., WinUI DispatcherQueue).
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Enqueues the specified action to run on the UI thread.
    /// </summary>
    /// <param name="action"> The action to run. </param>
    void Enqueue(Action action);
}
