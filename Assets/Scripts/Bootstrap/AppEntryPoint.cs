using System.Threading;
using Cysharp.Threading.Tasks;
using StateMachine;
using VContainer.Unity;

namespace Bootstrap
{
    public class AppEntryPoint : IAsyncStartable
    {
        private readonly IStatesController<AppState> _states;

        public AppEntryPoint(IStatesController<AppState> states)
        {
            _states = states;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            return _states.EnterStateAsync(AppState.Splash, cancellation);
        }
    }
}
