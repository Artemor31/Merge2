using CodeBase.Infrastructure;
using CodeBase.LevelData;
using UnityEngine;

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

        public void Enter()
        {
            // TODO!!!
            ServiceLocator.Resolve<ProgressService>().StaticData = Object.FindObjectOfType<LevelStaticData>();

            _sceneLoader.Load(GameplaySceneName, () => _gameStateMachine.Enter<GameLoopState>());
        }

        public void Exit()
        {
        }
    }
}