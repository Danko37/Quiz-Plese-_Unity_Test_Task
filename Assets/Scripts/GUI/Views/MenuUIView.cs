using GUI.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Views
{
    public class MenuUIView : View<MenuScreenViewModel>
    {
        [SerializeField] private Button _restartButton;

        public override void Bind(MenuScreenViewModel screenViewModel)
        {
            base.Bind(screenViewModel);

            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnRestartClicked()
        {
            GetViewModel.RestartCommand.Execute();
        }
    }
}
