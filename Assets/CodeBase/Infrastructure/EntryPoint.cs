using CodeBase.Services;
using UnityEngine;
using System;
using CodeBase.Gameplay;
using CodeBase.LevelData;
using CodeBase.Services.SaveService;
using CodeBase.Services.StateMachine;
using CodeBase.UI;

namespace CodeBase.Infrastructure
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
            CameraService cameraService = new(sceneLoader);
            GameFactory gameFactory = new(databaseProvider, assetsProvider, cameraService);
            RuntimeDataRepository runtimeDataRepository = new(gameFactory);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider);
            MergeService mergeService = new(gameFactory, databaseProvider);
            GameObserver gameObserver = new(runtimeDataRepository);
            GridService gridService = new(this, runtimeDataRepository, mergeService, cameraService);
            
            
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder, 
                runtimeDataRepository, gameFactory, gridService, runtimeDataRepository, gameObserver);

            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(runtimeDataRepository);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(mergeService);
            ServiceLocator.Bind(gameObserver);
            ServiceLocator.Bind(cameraService);
            
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