namespace Reactive
{
    public interface IReactiveProperty<T> : IObserved<T>
    {
        T Value { get; set; }
    }
}
