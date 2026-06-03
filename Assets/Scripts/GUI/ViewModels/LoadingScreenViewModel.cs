using System;
using Reactive;

namespace GUI.ViewModels
{
    public class LoadingScreenViewModel : ViewModel
    {
        public IObserved<float> Progress { get; }

        public LoadingScreenViewModel(IObserved<float> progress)
        {
            Progress = progress ?? throw new ArgumentNullException(nameof(progress));
            Progress.AddTo(Disposables);
        }
    }
}
