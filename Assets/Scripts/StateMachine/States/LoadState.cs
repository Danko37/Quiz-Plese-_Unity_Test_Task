using System.Threading;
using Cysharp.Threading.Tasks;
using GUI.Forms;
using GUI.ViewModels;
using Reactive;
using VContainer;

namespace StateMachine.States
{
    public class LoadState : IState
    {
        private const int StepCount = 5;
        private const int StepDelayMs = 200;

        private readonly IUIFormLoader _formLoader;
        private readonly IObjectResolver _resolver;

        private ReactiveProperty<float> _progress;
        private LoadingScreenViewModel _screenViewModel;

        public LoadState(IUIFormLoader formLoader, IObjectResolver resolver)
        {
            _formLoader = formLoader;
            _resolver = resolver;
        }

        public async UniTask EnterAsync(CancellationToken ct)
        {
            _progress = new ReactiveProperty<float>(0f);
            _screenViewModel = new LoadingScreenViewModel(_progress);
            _formLoader.ShowForm(_screenViewModel);

            for (var step = 1; step <= StepCount; step++)
            {
                await UniTask.Delay(StepDelayMs, cancellationToken: ct);
                _progress.Value = (float)step / StepCount;

                if (ct.IsCancellationRequested)
                {
                    break;
                }
            }

            var states = _resolver.Resolve<IStatesController<AppState>>();
            states.EnterStateAsync(AppState.Menu, ct).Forget();
        }

        public UniTask ExitAsync(CancellationToken ct)
        {
            _screenViewModel?.Hide();
            _screenViewModel = null;

            _progress = null;

            return UniTask.CompletedTask;
        }
    }
}
