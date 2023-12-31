﻿using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindowInstaller : MonoBehaviour
    {
        [SerializeField] private GameplayWindow _window;
        private GameplayWindowPresenter _presenter;

        private void Start()
        {
            if (_presenter != null) return;
            
            var progressService = ServiceLocator.Resolve<ProgressService>();
            var windowsService = ServiceLocator.Resolve<WindowsService>();
            var assetsProvider = ServiceLocator.Resolve<AssetsProvider>();
            var unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>()
                                              .GetDatabase<UnitsDatabase>();

            _presenter = new GameplayWindowPresenter(_window, windowsService, assetsProvider, 
                                                     unitsDatabase, progressService);
        }
    }
}