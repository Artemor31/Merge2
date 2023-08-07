using System;
using CodeBase.Databases;
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class EntryPoint : MonoBehaviour, ICoroutineRunner
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

            var assetsProvider = new AssetsProvider();
            var sceneLoader = new SceneLoader(this);
            
            ServiceLocator.Bind(sceneLoader);
            ServiceLocator.Bind(_windowsService);
            ServiceLocator.Bind(assetsProvider);
            ServiceLocator.Bind(this as ICoroutineRunner);
            ServiceLocator.Bind(new DatabaseProvider(assetsProvider));

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