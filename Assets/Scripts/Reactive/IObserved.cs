using System;

namespace Reactive
{
    public interface IObserved<T> : IDisposable
    {
        IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true);
        void Unsubscribe(Action<T> cb);
    }
}
