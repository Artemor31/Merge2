using System;
using Data;
using Gameplay;
using Gameplay.LevelItems;
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
            SaveService saveService = new();
            DatabaseProvider databaseProvider = new(assetsProvider);
            PlayerDataService playerService = new(saveService);
            CameraService cameraService = new(sceneLoader);
            GameFactory gameFactory = new(databaseProvider, assetsProvider, cameraService, this);
            GridDataService gridDataService = new(gameFactory, saveService);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider, playerService);
            MergeService mergeService = new(gameFactory, databaseProvider);
            GridViewService gridViewService = new(this, gridDataService, mergeService, cameraService);
            
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder, 
                gridDataService, gameFactory, gridViewService, gridDataService, playerService);

            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(saveService);
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(gridDataService);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(mergeService);
            ServiceLocator.Bind(cameraService);
            ServiceLocator.Bind(playerService);
            
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