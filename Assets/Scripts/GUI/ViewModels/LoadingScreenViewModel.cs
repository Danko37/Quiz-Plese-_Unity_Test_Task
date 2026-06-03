using System;
using Reactive;

namespace GUI.ViewModels
{
    /// <summary>
    /// Вью модель формы загрузки
    /// </summary>
    public class LoadingScreenViewModel : ViewModel
    {
        /// <summary>
        /// Слайдер загрузки
        /// </summary>
        public IObserved<float> Progress { get; }

        public LoadingScreenViewModel(IObserved<float> progress)
        {
            Progress = progress ?? throw new ArgumentNullException(nameof(progress));
            Progress.AddTo(Disposables);
        }
    }
}
