using CommunityToolkit.Mvvm.ComponentModel;
using DotNetMemoryLab.Presentation.State;

namespace DotNetMemoryLab.Presentation.ViewModels;

/// <summary>
/// Exposes heap breakdown (Gen0/Gen1/Gen2, LOH, POH) for charts.
/// </summary>
public sealed partial class HeapViewModel : ObservableObject, IObserver<UiState>, IDisposable
{
    private readonly UiStore _store;
    private IDisposable? _subscription;

    /// <summary>
    /// Initializes a new instance of <see cref="HeapViewModel"/>.
    /// </summary>
    /// <param name="store"> UI state store. </param>
    public HeapViewModel(UiStore store)
    {
        _store = store;
        _subscription = _store.Subscribe(this);

        if (_store.Latest is not null)
            Apply(_store.Latest);
    }

    [ObservableProperty] private long _gen0Bytes;
    [ObservableProperty] private long _gen1Bytes;
    [ObservableProperty] private long _gen2Bytes;
    [ObservableProperty] private long _lohBytes;
    [ObservableProperty] private long _pohBytes;

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
        var heap = state.Snapshot.Heap;

        Gen0Bytes = heap.Gen0.Size.Bytes;
        Gen1Bytes = heap.Gen1.Size.Bytes;
        Gen2Bytes = heap.Gen2.Size.Bytes;
        LohBytes = heap.Loh.Size.Bytes;
        PohBytes = heap.Poh.Size.Bytes;
    }
}
