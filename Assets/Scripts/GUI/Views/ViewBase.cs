using GUI.ViewModels;
using UnityEngine;

namespace GUI.Views
{
    public abstract class ViewBase : MonoBehaviour
    {
        public abstract void Bind(IViewModel viewModel);
        public abstract void Release();
    }
}
