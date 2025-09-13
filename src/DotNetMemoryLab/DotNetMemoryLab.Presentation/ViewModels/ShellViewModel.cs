using CommunityToolkit.Mvvm.ComponentModel;
using DotNetMemoryLab.Presentation.State;

namespace DotNetMemoryLab.Presentation.ViewModels;

/// <summary>
/// Root view model exposing KPI fields from the latest <see cref="UiState"/>.
/// </summary>
public sealed partial class ShellViewModel : ObservableObject, IObserver<UiState>, IDisposable
{
    private readonly UiStore _store;
    private IDisposable? _subscription;

    /// <summary>
    /// Initializes a new instance of <see cref="ShellViewModel"/>.
    /// </summary>
    /// <param name="store"> UI state store. </param>
    public ShellViewModel(UiStore store)
    {
        _store = store;
        _subscription = _store.Subscribe(this);

        if (_store.Latest is not null)
            Apply(_store.Latest);
    }

    [ObservableProperty] private long _workingSetBytes;
    [ObservableProperty] private long _privateBytes;
    [ObservableProperty] private long _totalManagedBytes;
    [ObservableProperty] private double _timeInGcPercent;

    /// <summary>
    /// Gets the most recent points for the total managed heap history (most recent first).
    /// </summary>
    public IReadOnlyList<long> TotalManagedHistory => _store.Latest?.TotalManagedHistory ?? [];

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
        WorkingSetBytes = state.Snapshot.Process.WorkingSet.Bytes;
        PrivateBytes = state.Snapshot.Process.PrivateBytes.Bytes;
        TotalManagedBytes = state.Snapshot.TotalManagedCommitted.Bytes;
        TimeInGcPercent = state.Snapshot.Gc.TimeInGcPercent.Value;

        OnPropertyChanged(nameof(TotalManagedHistory));
    }
}
