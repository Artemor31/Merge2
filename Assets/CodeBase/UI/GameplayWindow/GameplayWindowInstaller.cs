using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindowInstaller : MonoBehaviour
    {
        [SerializeField] private GameplayWindow _window;
        
        private GameplayModel _model;
        private GameplayWindowPresenter _presenter;

        private void OnEnable()
        {
            _model = ModelsContainer.Resolve<GameplayModel>();
            _presenter = new GameplayWindowPresenter(_model, _window);
        }
    }
}