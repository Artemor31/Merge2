using CodeBase.Infrastructure;
using CodeBase.Modules.Gameplay;
using CodeBase.StateMachine;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Modules.Menu
{
    public class MenuWindowInstaller : MonoBehaviour
    {
        [SerializeField] private MenuWindow _window;
        
        private PlayerModel _model;
        private MenuWindowPresenter _presenter;

        private void OnEnable()
        {
            var stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            
            _model = ModelsContainer.Resolve<PlayerModel>();
            _presenter = new MenuWindowPresenter(_model, _window, stateMachine);
        }
    }
}