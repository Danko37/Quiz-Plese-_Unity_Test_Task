using System;
using GUI.ViewModels;
using GUI.Views;
using UnityEngine;

namespace GUI.Forms
{
    /// <summary>
    /// Класс подгружает ui формы в зависимости от типа переданной модели представления.
    /// </summary>
    public class UIFormLoader : MonoBehaviour, IUIFormLoader
    {
        /// <summary>
        /// Ожидается, что имя префаба формы в папке Resources/Forms будет совпадать с именем типа модели представления без суффикса "ViewModel".
        /// </summary>
        public const string VIEW_MODEL_SUFFIX = "ViewModel";
        /// <summary>
        /// Путь до префабов ui относительно папки resources
        /// </summary>
        private const string FORMS_PATH = "Forms/";

        [SerializeField] private RectTransform _root;

        /// <summary>
        /// Метод подгружает и отображает ui форму. Вызывает байндинг
        /// </summary>
        /// <param name="viewModel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ShowForm(IViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var view = LoadFormView(viewModel);
            view.Bind(viewModel);
            AttachDisposeHandle(viewModel, view);
        }

        /// <summary>
        /// Метод подгружает и инстантейтит форму по типу вью модели. Возвращает вьюшку формы 
        /// </summary>
        private ViewBase LoadFormView(IViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_root == null)
                throw new InvalidOperationException($"{nameof(UIFormLoader)}: _root is not assigned in inspector.");

            var vmType = viewModel.GetType();
            var resourcePath = BuildResourcePath(vmType);

            //подгружаем форму из ресурсов. Это может быть как адрессаблс, так и самописный dlc загрузчик
            var prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab == null)
                throw new InvalidOperationException($"Prefab not found at Resources/{resourcePath} for {vmType.Name}.");

            var formInstance = Instantiate(prefab, _root, false);

            var view = formInstance.GetComponent<ViewBase>();
            if (view == null)
            {
                Destroy(formInstance);
                throw new InvalidOperationException($"Prefab at Resources/{resourcePath} has no {nameof(ViewBase)} component.");
            }

            return view;
        }

        /// <summary>
        /// Метод собирает данные для dispose и добавляет их 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="view"></param>
        private void AttachDisposeHandle(IViewModel viewModel, ViewBase view)
        {
            var handle = new FormDisposeHandle(view.gameObject, view, viewModel);
            (viewModel as ViewModel)?.SetHideHandle(handle);
        }

        /// <summary>
        /// Метод вырезает из имени типа модели представления суффикс "ViewModel" и строит путь до префаба формы относительно папки Resources/Forms.
        /// </summary>
        /// <param name="vmType"></param>
        /// <returns></returns>
        private static string BuildResourcePath(Type vmType)
        {
            var typeName = vmType.Name;
            return FORMS_PATH + typeName.Replace(VIEW_MODEL_SUFFIX, string.Empty);
        }

        /// <summary>
        /// Класс, который инкапсулирует в себе данные для корректного освобождения ресурсов формы.
        /// При вызове Dispose удаляет форму и вызывает метод Release у вьюшки, а также освобождает ресурсы модели представления.
        /// </summary>
        private class FormDisposeHandle : IDisposable
        {
            private GameObject _instance;
            private ViewBase _view;
            private IViewModel _viewModel;
            private bool _disposed;

            public FormDisposeHandle(GameObject instance, ViewBase view, IViewModel viewModel)
            {
                _instance = instance;
                _view = view;
                _viewModel = viewModel;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _disposed = true;

                try
                {
                    _view?.Release();
                }
                finally
                {
                    if (_instance != null)
                        Destroy(_instance);

                    var vm = _viewModel;
                    _view = null;
                    _instance = null;
                    _viewModel = null;

                    vm?.Dispose();
                }
            }
        }
    }
}
