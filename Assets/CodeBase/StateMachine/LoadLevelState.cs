using CodeBase.Services;

namespace CodeBase.StateMachine
{
    public class LoadLevelState : IState, IExitableState
    {
        private const string MenuSceneName = "Menu";
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
            _sceneLoader.Load(GameplaySceneName, () => _gameStateMachine.Enter<GameLoopState>());
        }

        public void Exit()
        {
            _sceneLoader.Unload(MenuSceneName);
        }
    }
}