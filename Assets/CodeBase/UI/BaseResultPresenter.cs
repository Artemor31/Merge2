using CodeBase.Infrastructure;
using CodeBase.Services.SaveService;
using CodeBase.Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class BaseResultPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Button _nextLevel;

        private GameStateMachine _gameStateMachine;
        private RuntimeDataRepository _runtimeRepo;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _runtimeRepo = ServiceLocator.Resolve<RuntimeDataRepository>();

            _moneyText.text = 100.ToString();
            _nextLevel.onClick.AddListener(OnNextLevelClicked);
        }

        private void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            _runtimeRepo.NextLevel();
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}