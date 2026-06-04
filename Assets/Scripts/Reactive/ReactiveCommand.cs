using System;
using System.Collections.Generic;

namespace Reactive
{
    public class ReactiveCommand : IReactiveCommand
    {
        private readonly List<Action> _subscribers = new();

        [NonSerialized] private bool _isDisposed;

        public IDisposable Subscribe(Action cb, bool invokeImmediately = true)
        {
            CheckDisposed();

            if (cb == null)
                throw new ArgumentNullException(nameof(cb));

            _subscribers.Add(cb);

            // invokeImmediately ignored: a command has no current value to replay.

            return new Subscription(this, cb);
        }

        public void Unsubscribe(Action cb)
        {
            if (_isDisposed)
                return;

            _subscribers.Remove(cb);
        }

        public void Execute()
        {
            CheckDisposed();

            var snapshot = new Action[_subscribers.Count];
            _subscribers.CopyTo(snapshot);

            foreach (var subscriber in snapshot)
            {
                subscriber();
            }
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ReactiveCommand));
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _subscribers.Clear();

            GC.SuppressFinalize(this);
        }

        ~ReactiveCommand()
        {
            Dispose();
        }

        private sealed class Subscription : IDisposable
        {
            private ReactiveCommand _owner;
            private Action _callback;

            public Subscription(ReactiveCommand owner, Action callback)
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

    /// <summary>
    /// Событие, которое может быть выполнено с параметром типа T.
    /// Подписчики будут уведомлены при вызове Execute с этим параметром.
    /// </summary>
    public class ReactiveCommand<T> : IReactiveCommand<T>
    {
        private readonly List<Action<T>> _subscribers = new();

        [NonSerialized] private bool _isDisposed;

        public IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true)
        {
            CheckDisposed();

            if (cb == null)
                throw new ArgumentNullException(nameof(cb));

            _subscribers.Add(cb);

            // invokeImmediately ignored: a command has no current value to replay.

            return new Subscription<T>(this, cb);
        }

        public void Unsubscribe(Action<T> cb)
        {
            if (_isDisposed)
                return;

            _subscribers.Remove(cb);
        }

        public void Execute(T parameter)
        {
            CheckDisposed();

            var snapshot = new Action<T>[_subscribers.Count];
            _subscribers.CopyTo(snapshot);

            foreach (var subscriber in snapshot)
            {
                subscriber(parameter);
            }
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ReactiveCommand<T>));
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _subscribers.Clear();

            //Просим GC не вызывать деструктор при сборке мусора
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Если кто то вручную не вызовет Dispose, то при сборке мусора будет вызван деструктор, который вызовет Dispose.
        /// </summary>
        ~ReactiveCommand()
        {
            Dispose();
        }
        
    }
}
