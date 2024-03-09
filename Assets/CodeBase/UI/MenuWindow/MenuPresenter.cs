using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.MenuWindow
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Button _playButton;
        
        private PlayerModel _playerModel;
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _playerModel = ServiceLocator.Resolve<ProgressService>().PlayerModel;

            _playerName.text = _playerModel.Name;
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