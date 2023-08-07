using CodeBase.Models;
using CodeBase.Services.StateMachine;

namespace CodeBase.UI.MenuWIndow
{
    public class MenuWindowPresenter
    {
        private readonly PlayerModel _playerModel;
        private readonly MenuWindow _menuWindow;
        private readonly GameStateMachine _gameStateMachine;

        public MenuWindowPresenter(PlayerModel playerModel, MenuWindow menuWindow, GameStateMachine gameStateMachine)
        {
            _playerModel = playerModel;
            _menuWindow = menuWindow;
            _gameStateMachine = gameStateMachine;

            _menuWindow.PlayerName.text = _playerModel.Name;
            _menuWindow.PlayButton.onClick.AddListener(PlayClicked);
        }

        private void PlayClicked()
        {
            _menuWindow.gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}