using GUI.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Views
{
    /// <summary>
    /// Вьюшка главного меню
    /// </summary>
    public class MenuUIView : View<MenuScreenViewModel>
    {
        [SerializeField] private Button _restartButton;

        public override void Bind(MenuScreenViewModel screenViewModel)
        {
            base.Bind(screenViewModel);

            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);
        }

        public override void Release()
        {
            base.Release();
            if (_restartButton != null)
                _restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        /// <summary>
        /// По нажатию на кнопку перезапускаем игру
        /// </summary>
        private void OnRestartClicked()
        {
            GetViewModel.RestartCommand.Execute();
        }
    }
}
