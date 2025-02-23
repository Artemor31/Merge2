using Infrastructure;
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

        private void OnRightClick()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<MenuState>();            
        }

        private void OnLeftClick() => gameObject.SetActive(false);
    }
}