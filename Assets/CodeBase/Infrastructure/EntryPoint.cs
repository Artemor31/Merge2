using System;
using Gameplay.Units;
using Services;
using Services.Buffs;
using Services.GridService;
using Services.Infrastructure;
using Services.Resources;
using Services.StateMachine;
using UI.UpgradeWindow;
using UnityEngine;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner, IUpdateable
    {
        public event Action Tick;

        [SerializeField] private WindowsService _windowsService;

        private static EntryPoint _instance;

        private void Start()
        {
            BindServices();

            if (_instance == null)
            {
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(_windowsService);
            }
            
            ServiceLocator.Resolve<GameStateMachine>().Enter<BootstrapState>();
        }

        private void BindServices()
        {
            ServiceLocator.Clear();

            AssetsProvider assetsProvider = new();
            SaveService saveService = new();
            SceneLoader sceneLoader = new(this);
            CameraService cameraService = new(sceneLoader);
            DatabaseProvider databaseProvider = new(assetsProvider);
            
            GameplayDataService gameplayService = new(saveService);
            PersistantDataService persistantDataService = new(saveService);
            GridDataService gridDataService = new(saveService);
            UpgradeDataService upgradeDataService = new(persistantDataService, saveService);

            GameFactory gameFactory = new(databaseProvider, assetsProvider, 
                cameraService, _windowsService, this);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider, gameplayService);
            
            GridLogicService gridLogicService = new(gridDataService, gameFactory, databaseProvider, 
                gameplayService, persistantDataService);
            MergeService mergeService = new(databaseProvider, gridLogicService);
            BuffService buffService = new(databaseProvider);
            
            SearchTargetService searchTargetService = new(gridDataService, waveBuilder);
            ProjectileService projectileService = new(this, databaseProvider);
            GridViewService gridViewService = new(this, gridDataService, mergeService, 
                cameraService, gridLogicService);
            
            GameStateMachine stateMachine = 
                new(sceneLoader, _windowsService, waveBuilder, 
                gridDataService, gridDataService, gameplayService, 
                gridLogicService, buffService, upgradeDataService, persistantDataService);

            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(saveService);
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(gridDataService);
            ServiceLocator.Bind(gridViewService);
            ServiceLocator.Bind(gridLogicService);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(mergeService);
            ServiceLocator.Bind(cameraService);
            ServiceLocator.Bind(gameplayService);
            ServiceLocator.Bind(buffService);
            ServiceLocator.Bind(persistantDataService);
            ServiceLocator.Bind(searchTargetService);
            ServiceLocator.Bind(projectileService);
            ServiceLocator.Bind(upgradeDataService);

            // game pipeline
            
            // menu
            // load level from data
            // start level in wait, show ui
            // buy stage
            // fight stage
            // wave result stage
            // load level stage (step 2) and repeat
            // or load menu stage
        }

        private void Update() => Tick?.Invoke();
    }
}