using GUI.ViewModels;
using Reactive;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Views
{
    public class LoadingUIView : View<LoadingScreenViewModel>
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _percentText;

        private readonly CompositeDisposable _disposables = new();

        public override void Bind(LoadingScreenViewModel screenViewModel)
        {
            base.Bind(screenViewModel);

            screenViewModel.Progress
                .Subscribe(OnProgressChanged)
                .AddTo(_disposables);
        }

        public override void Release()
        {
            _disposables.Dispose();
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
