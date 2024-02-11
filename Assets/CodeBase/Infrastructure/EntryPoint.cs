using CodeBase.Services;
using UnityEngine;
using System;
using CodeBase.Gameplay;

namespace CodeBase.Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner, IUpdateable
    {
        public event Action Tick;

        [SerializeField] private WindowsService _windowsService;
        [SerializeField] private LayerMask _gridLayerMask;

        private static EntryPoint _instance;
        private Game _game;

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

            _game = new Game(ServiceLocator.Resolve<SceneLoader>(), _windowsService);
        }

        private void BindServices()
        {
            ServiceLocator.Clear();

            var sceneLoader = new SceneLoader(this);
            var assetsProvider = new AssetsProvider();
            var progressService = new ProgressService();
            var databaseProvider = new DatabaseProvider(assetsProvider);
            var gameFactory = new GameFactory(databaseProvider, assetsProvider);
            var waveBuilder = new WaveBuilder(gameFactory, databaseProvider, progressService);
            var inputService = new InputService(this, _gridLayerMask);
            var gridService = new GridService(this, inputService);

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
        }

        private void Update() => Tick?.Invoke();
    }
}