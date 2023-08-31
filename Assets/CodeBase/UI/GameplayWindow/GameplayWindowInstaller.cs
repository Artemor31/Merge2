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

        private void Start()
        {
            if (_presenter != null) return;

            var levelStaticData = ModelsContainer.Resolve<LevelStaticData>();

            var windowsService = ServiceLocator.Resolve<WindowsService>();
            var assetsProvider = ServiceLocator.Resolve<AssetsProvider>();

            var unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>()
                                              .GetDatabase<UnitsDatabase>();

            _model = ModelsContainer.Resolve<GameplayModel>();
            _presenter = new GameplayWindowPresenter(_model, _window, windowsService,
                assetsProvider, unitsDatabase, levelStaticData);
        }
    }
}