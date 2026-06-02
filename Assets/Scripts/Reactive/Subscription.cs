using System;

namespace Reactive
{
    public class Subscription<T> : IDisposable
    {
        private IObserved<T> _owner;
        private Action<T> _callback;
        public Subscription(
            ReactiveProperty<T> owner,
            Action<T> callback)
        {
            _owner = owner;
            _callback = callback;
        }
        public void Dispose()
        {
            if (_owner == null)
                return;

            _owner.Unsubscribe(_callback);

            _owner = null;
            _callback = null;
        }
    }
}
