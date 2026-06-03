using GUI.ViewModels;
using Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Views
{
    /// <summary>
    /// Вью для отображения прогресса загрузки приложения.
    /// </summary>
    public class LoadingUIView : View<LoadingScreenViewModel>
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _percentText;

        public override void Bind(LoadingScreenViewModel screenViewModel)
        {
            base.Bind(screenViewModel);

            screenViewModel.Progress
                .Subscribe(OnProgressChanged)
                .AddTo(Disposables);
        }

        public override void Release()
        {
            Disposables.Dispose();
            base.Release();
        }

        private void OnProgressChanged(float value)
        {
            if (_progressBar != null)
                _progressBar.value = value;

            if (_percentText != null)
                _percentText.text = $"{Mathf.RoundToInt(value * 100f)}%";
        }
    }
}
