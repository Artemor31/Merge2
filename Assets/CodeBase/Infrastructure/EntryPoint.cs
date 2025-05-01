using System;
using System.Collections.Generic;
using Databases;
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
        [SerializeField] private BuffsDatabase BuffsDatabase;
        [SerializeField] private LevelDatabase LevelDatabase;
        [SerializeField] private ProjectilesDatabase ProjectilesDatabase;
        [SerializeField] private TutorTexts TutorTexts;
        [SerializeField] private UnitsDatabase UnitsDatabase;
        [SerializeField] private WaveRewardsDatabase WaveRewardsDatabase;
        [SerializeField] private WavesDatabase WavesDatabase;
        [SerializeField] private CurrencyDatabase CurrencyDatabase;
        [SerializeField] private LevelsDatabase LevelsDatabase;
        [SerializeField] private List<Database> _databases;

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
            DatabaseProvider databaseProvider = new(_databases);
            TutorialService tutorialService = new(saveService);
            GameplayContainer gameplayContainer = new();
            PersistantDataService persistantDataService = new(saveService);
            RewardsService rewardsService = new(persistantDataService, UnitsDatabase);
            GameplayDataService gameplayService = new(saveService);
            GridDataService gridDataService = new(saveService);
            UpgradeDataService upgradeDataService = new(persistantDataService, saveService);
            GameFactory gameFactory = new(databaseProvider, assetsProvider, _windowsService, this);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider);
            BuffService buffService = new(databaseProvider);
            GridService gridService = new(this, gridDataService, cameraService, gameFactory, databaseProvider, gameplayService, persistantDataService);
            SearchTargetService searchTargetService = new(gridDataService, waveBuilder);
            ProjectileService projectileService = new(this, databaseProvider);
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder,
                gridDataService, gridDataService, gameplayService,
                gridService, buffService, upgradeDataService,
                persistantDataService, this, projectileService, gameplayContainer, tutorialService);
            WaveRewardsService waveRewardsService = new(persistantDataService, databaseProvider, stateMachine, gameplayService, saveService, _windowsService);
            ActorRollService actorRollService = new(UnitsDatabase, persistantDataService, gameFactory, stateMachine, gameplayService);
            

            ServiceLocator.Bind(BuffsDatabase);
            ServiceLocator.Bind(LevelDatabase);
            ServiceLocator.Bind(ProjectilesDatabase);
            ServiceLocator.Bind(TutorTexts);
            ServiceLocator.Bind(UnitsDatabase);
            ServiceLocator.Bind(WaveRewardsDatabase);
            ServiceLocator.Bind(WavesDatabase);
            ServiceLocator.Bind(CurrencyDatabase);
            ServiceLocator.Bind(LevelsDatabase);

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
            ServiceLocator.Bind(actorRollService);
            ServiceLocator.Bind(rewardsService);
        }

        private void Update() => Tick?.Invoke();
    }
}