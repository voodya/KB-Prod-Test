using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniRx;
using VContainer;
using VContainer.Unity;

public class ABaseEntryPoint : IAsyncStartable
{
    private IEnumerable<IBootable> _bootables;
    private CompositeDisposable _compositeDisposable;

    protected CompositeDisposable Disposable => _compositeDisposable;

    [Inject]
    public ABaseEntryPoint(IEnumerable<IBootable> bootables)
    {
        _bootables = bootables;
    }

    public virtual async UniTask StartAsync(CancellationToken cancellation = default)
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
        var sorted = _bootables.OrderBy(x => x.Priority);
        foreach (var bootable in sorted)
        {
            await bootable.Boot();
        }
    }
}
