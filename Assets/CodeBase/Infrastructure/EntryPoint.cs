using System;
using Gameplay.Units;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using UnityEngine;
using YG;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner, IUpdateable
    {
        public event Action Tick;

        [SerializeField] private WindowsService _windowsService;

        private static EntryPoint _instance;

        private void Start()
        {
            string lang = YG2.lang;

            switch (lang)
            {
                case "tr":
                    YG2.SwitchLanguage("tr");
                    break;
                case "be" or "kk" or "uk" or "uz" or "ru":
                    YG2.SwitchLanguage("ru");
                    break;
                default:
                    YG2.SwitchLanguage("en");
                    break;
            }

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
            TutorialService tutorialService = new(saveService);
            GameplayContainer gameplayContainer = new();

            PersistantDataService persistantDataService = new(saveService);
            GameplayDataService gameplayService = new(saveService);
            GridDataService gridDataService = new(saveService, persistantDataService);
            UpgradeDataService upgradeDataService = new(persistantDataService, saveService);

            GameFactory gameFactory = new(databaseProvider, assetsProvider, _windowsService, this);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider);
            
            BuffService buffService = new(databaseProvider);
            GridService gridService = new(this, gridDataService, cameraService, 
                gameFactory, databaseProvider, gameplayService, persistantDataService);
            SearchTargetService searchTargetService = new(gridDataService, waveBuilder);
            ProjectileService projectileService = new(this, databaseProvider);

            
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder, 
                gridDataService, gridDataService, gameplayService, 
                gridService, buffService, upgradeDataService,
                persistantDataService, tutorialService, this, projectileService, gameplayContainer);
            
            
            WaveRewardsService waveRewardsService = new(persistantDataService, databaseProvider, stateMachine,
                gameplayService, saveService, _windowsService);

            ServiceLocator.Bind<ICoroutineRunner>(this);
            ServiceLocator.Bind<IUpdateable>(this);
            ServiceLocator.Bind(saveService);
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(gridDataService);
            ServiceLocator.Bind(gridService);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(cameraService);
            ServiceLocator.Bind(gameplayService);
            ServiceLocator.Bind(buffService);
            ServiceLocator.Bind(persistantDataService);
            ServiceLocator.Bind(searchTargetService);
            ServiceLocator.Bind(projectileService);
            ServiceLocator.Bind(upgradeDataService);
            ServiceLocator.Bind(tutorialService);
            ServiceLocator.Bind(gameplayContainer);
            ServiceLocator.Bind(waveRewardsService);
        }

        private void Update() => Tick?.Invoke();
    }
}