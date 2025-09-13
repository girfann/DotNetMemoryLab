using CommunityToolkit.Mvvm.ComponentModel;
using DotNetMemoryLab.Presentation.State;

namespace DotNetMemoryLab.Presentation.ViewModels;

/// <summary>
/// Exposes stack-related info (thread count and estimated total stack reserve).
/// </summary>
public sealed partial class StackViewModel : ObservableObject, IObserver<UiState>, IDisposable
{
    private readonly UiStore _store;
    private IDisposable? _subscription;

    /// <summary>
    /// Initializes a new instance of <see cref="StackViewModel"/>.
    /// </summary>
    /// <param name="store"> UI state store. </param>
    public StackViewModel(UiStore store)
    {
        _store = store;
        _subscription = _store.Subscribe(this);

        if (_store.Latest is not null)
            Apply(_store.Latest);
    }

    [ObservableProperty] private int _totalThreads;
    [ObservableProperty] private long _estimatedTotalStackReserveBytes;

    /// <inheritdoc />
    public void OnNext(UiState value) => Apply(value);

    /// <inheritdoc />
    public void OnError(Exception error) { }

    /// <inheritdoc />
    public void OnCompleted() { }

    /// <summary>
    /// Disposes the subscription to the store.
    /// </summary>
    public void Dispose()
    {
        _subscription?.Dispose();
        _subscription = null;
    }

    private void Apply(UiState state)
    {
        var threads = state.Snapshot.Threads;

        TotalThreads = threads.TotalThreads;
        EstimatedTotalStackReserveBytes = threads.EstimatedTotalStackReserve.Bytes;
    }
}
