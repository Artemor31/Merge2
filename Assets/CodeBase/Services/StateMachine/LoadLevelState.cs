using System;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
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

        public void Enter()
        {
            _sceneLoader.Load(GameplaySceneName, () =>
            {
                // TODO!!!
                var staticData = Object.FindObjectOfType<LevelStaticData>();
                if (staticData == null)
                    throw new Exception("Static data not found");
                
                ServiceLocator.Resolve<ProgressService>().StaticData = staticData;
                _gameStateMachine.Enter<GameLoopState>();
            });
        }

        public void Exit()
        {
        }
    }
}