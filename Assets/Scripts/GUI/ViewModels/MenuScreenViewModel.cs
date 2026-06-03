using Reactive;

namespace GUI.ViewModels
{
    public class MenuScreenViewModel : ViewModel
    {
        private readonly ReactiveCommand _restartCommand;

        public IReactiveCommand RestartCommand => _restartCommand;
        
        public MenuScreenViewModel()
        {
            _restartCommand = new ReactiveCommand().AddTo(Disposables);
        }
    }
}
