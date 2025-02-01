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
            TutorialService tutorialService = new();

            PersistantDataService persistantDataService = new(saveService);
            GameplayDataService gameplayService = new(saveService, persistantDataService);
            GridDataService gridDataService = new(saveService, persistantDataService);
            UpgradeDataService upgradeDataService = new(persistantDataService, saveService);

            GameFactory gameFactory = new(databaseProvider, assetsProvider, _windowsService, this);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider, gameplayService, tutorialService);
            
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
                gridLogicService, buffService, upgradeDataService, persistantDataService, tutorialService);

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
            ServiceLocator.Bind(tutorialService);
        }

        private void Update() => Tick?.Invoke();
    }
}