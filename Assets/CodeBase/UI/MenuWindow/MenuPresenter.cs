using Infrastructure;
using Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuWindow
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Button _playButton;
        
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();

            _playButton.onClick.AddListener(PlayClicked);
        }

        private void PlayClicked()
        {
            gameObject.SetActive(false);
            _playButton.onClick.RemoveListener(PlayClicked);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}