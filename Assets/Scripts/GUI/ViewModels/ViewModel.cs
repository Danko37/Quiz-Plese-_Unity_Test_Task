using System;
using Reactive;

namespace GUI.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        protected readonly CompositeDisposable Disposables = new();

        private IDisposable _hideHandle;
        private bool _disposed;

        internal void SetHideHandle(IDisposable handle)
        {
            _hideHandle = handle;
        }

        public void Hide()
        {
            if (_disposed)
                return;

            if (_hideHandle != null)
            {
                var handle = _hideHandle;
                _hideHandle = null;
                handle.Dispose();
            }

            Dispose();
        }

        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            Disposables.Dispose();
        }
    }
}
