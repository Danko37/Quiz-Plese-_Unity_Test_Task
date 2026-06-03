using System.Threading;
using Cysharp.Threading.Tasks;
using StateMachine;
using VContainer.Unity;

namespace Bootstrap
{
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    public class AppEntryPoint : IAsyncStartable
    {
        /// <summary>
        /// Сосотояния приложения
        /// </summary>
        private readonly IStatesController<AppState> _states;

        public AppEntryPoint(IStatesController<AppState> states)
        {
            _states = states;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            //первое состояние - Показ эмблемы
            return _states.EnterStateAsync(AppState.Splash, cancellation);
        }
    }
}
