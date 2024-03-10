using CodeBase.Services;
using UnityEngine;
using System;
using CodeBase.Gameplay;
using CodeBase.Services.StateMachine;

namespace CodeBase.Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner, IUpdateable
    {
        public event Action Tick;

        [SerializeField] private WindowsService _windowsService;
        [SerializeField] private LayerMask _gridLayerMask;

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

            var sceneLoader = new SceneLoader(this);
            var assetsProvider = new AssetsProvider();
            var progressService = new ProgressService();
            var databaseProvider = new DatabaseProvider(assetsProvider);
            var gameFactory = new GameFactory(databaseProvider, assetsProvider);
            var waveBuilder = new WaveBuilder(gameFactory, databaseProvider);
            var inputService = new InputService(this, _gridLayerMask);
            var gridService = new GridService(this, inputService);
            var battleConductor = new BattleConductor(progressService, waveBuilder);
            var stateMachine = new GameStateMachine(sceneLoader, _windowsService, waveBuilder, battleConductor);

            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(inputService);
            ServiceLocator.Bind(progressService);
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(waveBuilder);
            ServiceLocator.Bind(gridService);
            ServiceLocator.Bind(stateMachine);
            ServiceLocator.Bind(battleConductor);

            // game pipeline
            
            // menu
            // load level from data
            // start level in wait, show ui
            // buy stage
            // fight stage
            // wave result stage
            // load level stage (step 2) and repeat
            // or load menu stage
            
            
            _windowsService.InitWindows();
        }

        private void Update() => Tick?.Invoke();
    }
}