using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Modules.Gameplay
{
    public class GameplayWindowInstaller : MonoBehaviour
    {
        [SerializeField] private GameplayWindow _window;
        
        private IGameplayModel _model;
        private GameplayWindowPresenter _presenter;

        private void OnEnable()
        {
            _model = ModelsContainer.Resolve<IGameplayModel>();
            _presenter = new GameplayWindowPresenter(_model, _window);
        }
    }
}