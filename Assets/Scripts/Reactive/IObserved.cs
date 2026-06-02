using System;

namespace Reactive
{ 
    public interface IObserved<T>
    {
        event Action<T> OnChanged;
        IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true);
        void Unsubscribe(Action<T> cb);
    }
}
