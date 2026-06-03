using System;
using System.Collections.Generic;

namespace Reactive
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private readonly List<Action<T>> _subscribers = new();

        [NonSerialized] private bool _isDisposed = false;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                CheckDisposed();

                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;

                NotifySubscribers();
            }
        }

        private void NotifySubscribers()
        {
            var snapshot = new Action<T>[_subscribers.Count];
            _subscribers.CopyTo(snapshot);
            foreach (var subscriber in snapshot)
            {
                subscriber(_value);
            }
        }

        private void CheckDisposed()
        {
            if(_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ReactiveProperty<T>));
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
        
        protected virtual void SetValue(T value)
        {
            _value = value;
        }

        public IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true)
        {
            CheckDisposed();
            
            if (cb == null)
                throw new ArgumentNullException(nameof(cb));

            _subscribers.Add(cb);

            if (invokeImmediately)
            {
                cb(_value);
            }

            return new Subscription<T>(this, cb);
        }

        public void Unsubscribe(Action<T> cb)
        {
            if (_isDisposed)
                return;

            _subscribers.Remove(cb);
        }

        public void SetValueAndForceNotify(T value)
        {
            CheckDisposed();

            _value = value;

            NotifySubscribers();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _subscribers.Clear();
            
            GC.SuppressFinalize(this);
        }

        ~ReactiveProperty()
        {
            Dispose();
        }
    }
}