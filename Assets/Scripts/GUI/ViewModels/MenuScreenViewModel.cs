using Reactive;

namespace GUI.ViewModels
{
    /// <summary>
    /// Вью модель формы меню
    /// </summary>
    public class MenuScreenViewModel : ViewModel
    {
        private readonly ReactiveCommand _restartCommand;

        /// <summary>
        /// Команда перезагрузки игры
        /// </summary>
        public IReactiveCommand RestartCommand => _restartCommand;
        
        public MenuScreenViewModel()
        {
            _restartCommand = new ReactiveCommand().AddTo(Disposables);
        }
    }
}
