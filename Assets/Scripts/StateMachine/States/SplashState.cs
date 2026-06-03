using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GUI.Forms;
using GUI.ViewModels;
using VContainer;

namespace StateMachine.States
{
    public class SplashState : IState
    {
        private static readonly TimeSpan SplashDuration = TimeSpan.FromSeconds(1);

        private readonly IUIFormLoader _formLoader;
        private readonly IObjectResolver _resolver;

        private SplashScreenViewModel _screenViewModel;

        public SplashState(IUIFormLoader formLoader, IObjectResolver resolver)
        {
            _formLoader = formLoader;
            _resolver = resolver;
        }

        public async UniTask EnterAsync(CancellationToken ct)
        {
            _screenViewModel = new SplashScreenViewModel();
            _formLoader.ShowForm(_screenViewModel);

            await UniTask.Delay(SplashDuration, cancellationToken: ct);

            var states = _resolver.Resolve<IStatesController<AppState>>();
            states.EnterStateAsync(AppState.Load, ct).Forget();
        }

        public UniTask ExitAsync(CancellationToken ct)
        {
            _screenViewModel?.Hide();
            _screenViewModel = null;
            return UniTask.CompletedTask;
        }
    }
}
