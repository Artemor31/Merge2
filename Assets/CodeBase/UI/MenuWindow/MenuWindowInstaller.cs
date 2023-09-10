using CodeBase.Infrastructure;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using UnityEngine;

namespace CodeBase.UI.MenuWindow
{
    public class MenuWindowInstaller : MonoBehaviour
    {
        [SerializeField] private MenuWindow _window;
        private MenuWindowPresenter _presenter;

        private void OnEnable()
        {
            var stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            var playerModel = ServiceLocator.Resolve<ProgressService>().PlayerModel;
            
            _presenter = new MenuWindowPresenter(playerModel, _window, stateMachine);
        }
    }
}