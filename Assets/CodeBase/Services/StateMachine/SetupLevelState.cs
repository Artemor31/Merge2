using Gameplay.Grid;
using Services.Infrastructure;
using UI.GameplayWindow;
using UI.WorldSpace;

namespace Services.StateMachine
{
    public class SetupLevelState : IExitableState
    {
        private readonly WindowsService _windowsService;
        private readonly GridService _gridLogicService;
        private readonly GameplayContainer _gameplayContainer;

        public SetupLevelState(WindowsService windowsService, GridService gridLogicService, GameplayContainer gameplayContainer)
        {
            _windowsService = windowsService;
            _gridLogicService = gridLogicService;
            _gameplayContainer = gameplayContainer;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();
            _windowsService.Show<ShopPresenter>();
            _windowsService.Show<GameCanvas, GameCanvasData>(new GameCanvasData(true));
            _gridLogicService.EnableGridView(true);
            _gameplayContainer.Get<EnemyGrid>().gameObject.SetActive(true);
        }

        public void Exit()
        {
            _gridLogicService.EnableGridView(false);
            _gameplayContainer.Get<EnemyGrid>().gameObject.SetActive(false);
            
            _windowsService.Close<GameplayPresenter>();
            _windowsService.Close<BuffInfoPresenter>();
        }
    }
}