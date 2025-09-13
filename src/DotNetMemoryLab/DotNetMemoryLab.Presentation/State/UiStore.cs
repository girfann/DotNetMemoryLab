using DotNetMemoryLab.Diagnostics.Abstractions;
using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Options;
using DotNetMemoryLab.Presentation.Abstractions;

namespace DotNetMemoryLab.Presentation.State;

/// <summary>
/// Consumes <see cref="IMemoryMetricsProvider"/> and projects it to <see cref="UiState"/>.
/// Also marshals updates to the UI thread using <see cref="IDispatcher"/>.
/// </summary>
public sealed class UiStore : IObservable<UiState>, IDisposable
{
    private readonly IDispatcher _dispatcher;
    private readonly int _historyCapacity;
    private readonly List<IObserver<UiState>> _observers = [];

    private IDisposable? _subscription;

    /// <summary>
    /// Initializes a new instance of the store.
    /// </summary>
    /// <param name="provider"> The diagnostics metrics provider. </param>
    /// <param name="dispatcher"> Dispatcher to marshal updates to UI thread. </param>
    /// <param name="options"> UI expectations for history length. </param>
    public UiStore(
        IMemoryMetricsProvider provider,
        IDispatcher dispatcher,
        MetricsOptions options)
    {
        _dispatcher = dispatcher;
        _historyCapacity = Math.Max(1, options.HistoryPoints);

        _subscription = provider.Subscribe(new SnapshotObserver(this));
    }

    /// <summary>
    /// Gets the latest published UI state.
    /// </summary>
    public UiState? Latest { get; private set; }

    /// <inheritdoc />
    public IDisposable Subscribe(IObserver<UiState> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Add(observer);

        if (Latest is not null)
            observer.OnNext(Latest);

        return new Unsubscriber(_observers, observer);
    }

    /// <summary>
    /// Disposes subscriptions and clears observers.
    /// </summary>
    public void Dispose()
    {
        _subscription?.Dispose();
        _subscription = null;
        _observers.Clear();
    }

    private void OnDomainSnapshot(MemorySnapshot snapshot)
    {
        // Build UiState and push managed total history
        var state = new UiState
        {
            Snapshot = snapshot,
            HistoryCapacity = _historyCapacity,
            TotalManagedHistory = Latest?.TotalManagedHistory ?? []
        }.WithPushedManaged(snapshot.TotalManagedCommitted);

        Latest = state;

        // Publish to observers on UI thread
        if (_observers.Count == 0)
            return;

        _dispatcher.Enqueue(() =>
        {
            foreach (var o in _observers.ToArray())
                o.OnNext(state);
        });
    }

    private sealed class SnapshotObserver(UiStore owner) : IObserver<MemorySnapshot>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(MemorySnapshot value) => owner.OnDomainSnapshot(value);
    }

    private sealed class Unsubscriber(List<IObserver<UiState>> list, IObserver<UiState> observer) : IDisposable
    {
        public void Dispose() => list.Remove(observer);
    }
}
