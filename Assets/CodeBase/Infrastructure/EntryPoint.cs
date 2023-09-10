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

        private static EntryPoint _instance;
        private Game _game;

        private void Start() =>
            CreateGame();

        public void CreateGame()
        {
            ServiceLocator.Clear();
            ModelsContainer.Clear();

            var sceneLoader = new SceneLoader(this);

            var assetsProvider = new AssetsProvider();
            var databaseProvider = new DatabaseProvider(assetsProvider);
            var gameFactory = new GameFactory(databaseProvider, assetsProvider);

            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(databaseProvider);
            ServiceLocator.Bind(this as IUpdateable);
            ServiceLocator.Bind(new InputService(this));
            ServiceLocator.Bind(new ProgressService());
            ServiceLocator.Bind(gameFactory);
            ServiceLocator.Bind(new WaveBuilder(gameFactory, databaseProvider));

            if (_instance == null)
            {
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(_windowsService);
            }

            _game = new Game(sceneLoader, _windowsService);
        }

        private void Update()
        {
            Tick?.Invoke();
        }
    }
}