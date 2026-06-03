namespace Reactive
{
    public interface IReactiveCommand : IObserved
    {
        void Execute();
    }

    public interface IReactiveCommand<T> : IObserved<T>
    {
        void Execute(T parameter);
    }
}
