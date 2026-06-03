using System;

namespace Reactive
{
    public interface IReactiveCommand
    {
        IDisposable Subscribe(Action cb);
        void Unsubscribe(Action cb);
        void Execute();
    }

    public interface IReactiveCommand<T>
    {
        IDisposable Subscribe(Action<T> cb);
        void Unsubscribe(Action<T> cb);
        void Execute(T parameter);
    }
}
