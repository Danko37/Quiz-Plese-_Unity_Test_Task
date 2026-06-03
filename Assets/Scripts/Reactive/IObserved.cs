using System;

namespace Reactive
{
    /// <summary>
    /// Базовый интерфейс для всех наблюдаемых объектов.
    /// </summary>
    public interface IObserved : IDisposable
    {
        IDisposable Subscribe(Action cb, bool invokeImmediately = true);
        void Unsubscribe(Action cb);
    }

    /// <summary>
    /// Базовый интерфейс для всех наблюдаемых объектов с параметром типа значения.
    /// </summary>
    public interface IObserved<T> : IDisposable
    {
        IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true);
        void Unsubscribe(Action<T> cb);
    }
}
