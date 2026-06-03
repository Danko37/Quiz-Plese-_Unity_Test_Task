using System;

namespace Reactive
{
    public static class DisposableExtensions
    {
        /// <summary>
        /// Метод расширения для объектов IDisposable, который добавляет их в CompositeDisposable.
        /// </summary>
        public static T AddTo<T>(
            this T disposable,
            CompositeDisposable composite)
            where T : IDisposable
        {
            composite.Add(disposable);
            return disposable;
        }
    }
}