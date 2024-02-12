using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.StateMachine
{
    public class LoadLevelState : IState, IExitableState
    {
        private const string GameplaySceneName = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName, 
                 () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            var staticData = Object.FindObjectOfType<LevelStaticData>();
            IReadOnlyList<Vector3> _ = staticData.EnemyPositions;
            ServiceLocator.Resolve<ProgressService>().StaticData = staticData;
            ServiceLocator.Resolve<InputService>().SetCamera(Camera.main);
            ServiceLocator.Resolve<GridService>().Init();
        }
    }
}