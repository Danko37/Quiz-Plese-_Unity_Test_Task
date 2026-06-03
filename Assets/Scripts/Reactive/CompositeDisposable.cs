using System;
using System.Collections.Generic;

namespace Reactive
{
    /// <summary>
    /// Собирает в себе объекты для диспозинга.
    /// Удобно для управления временем жизни объектов, например, подписок на команды.
    /// </summary>
    public class CompositeDisposable
    {
        private readonly List<IDisposable> _items = new();
        private bool _disposed;

        public void Add(IDisposable disposable)
        {
            if (disposable == null)
                return;

            if (_disposed)
            {
                disposable.Dispose();
                return;
            }

            _items.Add(disposable);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (var item in _items)
            {
                item.Dispose();
            }

            _items.Clear();
        }
    }
}
