using Infrastructure;
using Services.Infrastructure;
using Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResultWindow
{
    public class CloseConfirmPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _right;
        [SerializeField] private Button _left;
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            
            _right.onClick.AddListener(OnRightClick);
            _left.onClick.AddListener(OnLeftClick);
        }

        private void OnLeftClick() => ServiceLocator.Resolve<WindowsService>().Close<CloseConfirmPresenter>();

        private void OnRightClick()
        {
            OnLeftClick();
            _gameStateMachine.Enter<ResultScreenState, ResultScreenData>(new ResultScreenData(false, true));
        }
    }
}