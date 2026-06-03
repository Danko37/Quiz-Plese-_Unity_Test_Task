using GUI.ViewModels;
using UnityEngine;

namespace GUI.Views
{
    /// <summary>
    /// Базовый класс для всех вьюшек
    /// </summary>
    public abstract class ViewBase : MonoBehaviour
    {
        /// <summary>
        /// Привязывает вью к вьюмодели.
        /// В этом методе должны быть подписки на события вьюмодели и установка начального состояния вью.
        /// </summary>
        public abstract void Bind(IViewModel viewModel);
        
        /// <summary>
        /// Метод вызывает при уничтожении вьюшки.
        /// В нем должны быть отписки от событий вьюмодели и освобождение ресурсов, если это необходимо.
        /// </summary>
        public abstract void Release();

        protected virtual void OnDestroy()
        {
            Release();
        }
    }
}
