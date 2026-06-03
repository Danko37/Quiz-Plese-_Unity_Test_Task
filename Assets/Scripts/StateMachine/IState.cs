using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IState
    {
        UniTask EnterAsync(CancellationToken ct);
        UniTask ExitAsync(CancellationToken ct);
    }
}
