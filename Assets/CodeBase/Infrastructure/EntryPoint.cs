using System;
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

        private void Start()
        {
            CreateGame();
        }

        public void CreateGame()
        {
            BindServices();
            CheckForSingleton();
            _game = new Game(ServiceLocator.Resolve<SceneLoader>(), ServiceLocator.Resolve<WindowsService>());
        }

        private void CheckForSingleton()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(_windowsService);
            }
        }

        private void BindServices()
        {
            ServiceLocator.Clear();

            ServiceLocator.Bind(new SceneLoader(this));
            ServiceLocator.Bind(_windowsService);
        }

        private void Update()
        {
            Tick?.Invoke();
        }
    }
}