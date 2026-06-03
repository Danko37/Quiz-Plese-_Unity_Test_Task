using System;
using GUI.ViewModels;

namespace GUI.Views
{
    public abstract class View<TViewModel> : ViewBase where TViewModel : class, IViewModel
    {
        private TViewModel _viewModel;

        //сюда суется вью модель
        public sealed override void Bind(IViewModel viewModel)
        {
            if (viewModel is not TViewModel typed)
                throw new ArgumentException(
                    $"{GetType().Name} expects {typeof(TViewModel).Name}, got {viewModel?.GetType().Name ?? "null"}.",
                    nameof(viewModel));

            Bind(typed);
        }

        //здесь дергается при байндинге
        public virtual void Bind(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Release()
        {
        }

        public TViewModel GetViewModel => _viewModel ?? throw new InvalidOperationException("ViewModel is not set for " + this);
    }
}
