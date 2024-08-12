using System;
using Gameplay;
using LevelData;
using Services;
using Services.SaveService;
using Services.StateMachine;
using UI;
using UnityEngine;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner, IUpdateable
    {
        public event Action Tick;

        [SerializeField] private WindowsService _windowsService;

        private static EntryPoint _instance;

        private void Start() =>
            CreateGame();

        private void CreateGame()
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

            SceneLoader sceneLoader = new(this);
            AssetsProvider assetsProvider = new();
            DatabaseProvider databaseProvider = new(assetsProvider);
            RepositoryProvider repositoryProvider = new();
            PlayerProgressService playerService = new(repositoryProvider);
            CameraService cameraService = new(sceneLoader);
            GameFactory gameFactory = new(databaseProvider, assetsProvider, cameraService, this);
            GridDataService gridDataService = new(gameFactory);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider, playerService, gridDataService);
            MergeService mergeService = new(gameFactory, databaseProvider);
            GridService gridService = new(this, gridDataService, mergeService, cameraService);
            GameObserver gameObserver = new(gridDataService, playerService);
            
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder, 
                gridDataService, gameFactory, gridService, gridDataService, gameObserver, playerService);

            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(gridDataService);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(mergeService);
            ServiceLocator.Bind(gameObserver);
            ServiceLocator.Bind(cameraService);
            ServiceLocator.Bind(playerService);
            ServiceLocator.Bind(repositoryProvider);
            
            _windowsService.InitWindows();

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