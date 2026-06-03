using System;

namespace Reactive
{
    /// <summary>
    /// Класс описывает подписку и её диспоз
    /// </summary>
    public class Subscription<T> : IDisposable
    {
        private IObserved<T> _owner;
        private Action<T> _callback;
        public Subscription(IObserved<T> owner, Action<T> callback)
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
