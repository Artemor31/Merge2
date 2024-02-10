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

        private void Start()
        {
            var stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            var progressService = ServiceLocator.Resolve<ProgressService>();
            _presenter = new MenuWindowPresenter(progressService.PlayerModel, _window, stateMachine);
        }
    }
}