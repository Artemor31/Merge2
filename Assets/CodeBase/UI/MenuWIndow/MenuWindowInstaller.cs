using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services.StateMachine;
using UnityEngine;

namespace CodeBase.UI.MenuWIndow
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