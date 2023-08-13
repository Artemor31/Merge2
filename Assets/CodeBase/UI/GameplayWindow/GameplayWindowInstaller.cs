using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services;
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
            if (_presenter != null) return;
            
            var windowsService = ServiceLocator.Resolve<WindowsService>();
            var assetsProvider = ServiceLocator.Resolve<AssetsProvider>();
            var unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            var inputService = ServiceLocator.Resolve<InputService>();

            _model = ModelsContainer.Resolve<GameplayModel>();
            _presenter = new GameplayWindowPresenter(_model, _window, windowsService, assetsProvider, unitsDatabase, inputService);
        }
    }
}