using Infrastructure;
using Services.Infrastructure;
using Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _addHardButton;
        [SerializeField] private Button _addSoftButton;
        
        [SerializeField] private TextMeshProUGUI _softText;
        [SerializeField] private TextMeshProUGUI _hardText;
        
        private GameStateMachine _gameStateMachine;
        private WindowsService _windowService;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _windowService = ServiceLocator.Resolve<WindowsService>();

            _fightButton.onClick.AddListener(PlayClicked);
        }

        private void PlayClicked()
        {
            gameObject.SetActive(false);
            _fightButton.onClick.RemoveListener(PlayClicked);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}