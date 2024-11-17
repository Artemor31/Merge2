using System;
using Data;
using Gameplay;
using Gameplay.LevelItems;
using Services;
using Services.BuffService;
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

            AssetsProvider assetsProvider = new();
            SaveService saveService = new();
            SceneLoader sceneLoader = new(this);
            CameraService cameraService = new(sceneLoader);
            DatabaseProvider databaseProvider = new(assetsProvider);
            PlayerDataService playerService = new(saveService);

            GameFactory gameFactory = new(databaseProvider, assetsProvider, cameraService, this);
            WaveBuilder waveBuilder = new(gameFactory, databaseProvider, playerService);
            
            GridDataService gridDataService = new(saveService);
            GridLogicService gridLogicService = new(gridDataService, gameFactory);
            MergeService mergeService = new(databaseProvider, gridLogicService);
            GridViewService gridViewService = new(this, gridDataService, mergeService, cameraService);
            
            GameStateMachine stateMachine = new(sceneLoader, _windowsService, waveBuilder, 
                gridDataService, gridDataService, playerService, gridLogicService);

            BuffService buffService = new();
            BuffViewService buffViewService = new(buffService, gridLogicService, stateMachine, gridDataService);

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
            ServiceLocator.Bind(playerService);
            ServiceLocator.Bind(buffService);
            ServiceLocator.Bind(buffViewService);

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