using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IStatesController<TEnum>
    {
        UniTask EnterStateAsync(TEnum code, CancellationToken ct);
    }
}
