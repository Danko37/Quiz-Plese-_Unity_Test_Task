using System;
using GUI.ViewModels;
using GUI.Views;
using UnityEngine;

namespace GUI.Forms
{
    public class UIFormLoader : MonoBehaviour, IUIFormLoader
    {
        private const string VIEW_MODEL_SUFFIX = "ViewModel";
        private const string FORMS_PATH = "Forms/";

        [SerializeField] private RectTransform _root;

        public void ShowForm(IViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var view = LoadFormView(viewModel);
            view.Bind(viewModel);
            AttachHandle(viewModel, view);
        }

        private ViewBase LoadFormView(IViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_root == null)
                throw new InvalidOperationException($"{nameof(UIFormLoader)}: _root is not assigned in inspector.");

            var vmType = viewModel.GetType();
            var resourcePath = BuildResourcePath(vmType);

            var prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab == null)
                throw new InvalidOperationException($"Prefab not found at Resources/{resourcePath} for {vmType.Name}.");

            var instance = Instantiate(prefab, _root, false);

            var view = instance.GetComponent<ViewBase>();
            if (view == null)
            {
                Destroy(instance);
                throw new InvalidOperationException($"Prefab at Resources/{resourcePath} has no {nameof(ViewBase)} component.");
            }

            return view;
        }

        protected void AttachHandle(IViewModel viewModel, ViewBase view)
        {
            var handle = new FormHandle(view.gameObject, view, viewModel);
            (viewModel as ViewModel)?.SetHideHandle(handle);
        }

        private static string BuildResourcePath(Type vmType)
        {
            var typeName = vmType.Name;
            return FORMS_PATH + typeName.Replace(VIEW_MODEL_SUFFIX, string.Empty);
        }

        private sealed class FormHandle : IDisposable
        {
            private GameObject _instance;
            private ViewBase _view;
            private IViewModel _viewModel;
            private bool _disposed;

            public FormHandle(GameObject instance, ViewBase view, IViewModel viewModel)
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
