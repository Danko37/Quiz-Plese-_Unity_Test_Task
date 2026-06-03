using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GUI.Forms;
using GUI.ViewModels;
using VContainer;

namespace StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IUIFormLoader _formLoader;
        private readonly IObjectResolver _resolver;

        private MenuScreenViewModel _screenViewModel;
        private IDisposable _restartSubscription;
        private CancellationToken _ct;
        private bool _restartTriggered;

        public MenuState(IUIFormLoader formLoader, IObjectResolver resolver)
        {
            _formLoader = formLoader;
            _resolver = resolver;
        }

        public UniTask EnterAsync(CancellationToken ct)
        {
            _ct = ct;
            _restartTriggered = false;

            _screenViewModel = new MenuScreenViewModel();
            _restartSubscription = _screenViewModel.RestartCommand.Subscribe(OnRestartRequested);

            _formLoader.ShowForm(_screenViewModel);

            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken ct)
        {
            _restartSubscription?.Dispose();
            _restartSubscription = null;

            _screenViewModel?.Hide();
            _screenViewModel = null;

            return UniTask.CompletedTask;
        }

        private void OnRestartRequested()
        {
            if (_restartTriggered)
                return;

            _restartTriggered = true;

            var states = _resolver.Resolve<IStatesController<AppState>>();
            states.EnterStateAsync(AppState.Load, _ct).Forget();
        }
    }
}
