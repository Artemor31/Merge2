using CodeBase.Services;

namespace CodeBase.StateMachine
{
    public class BootstrapState : IState
    {
        private const string MenuSceneName = "Menu";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(MenuSceneName, () => _gameStateMachine.Enter<MenuState>());
        }
    }
}