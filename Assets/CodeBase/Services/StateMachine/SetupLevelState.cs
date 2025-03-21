using Gameplay.Grid;
using Services.GridService;
using Services.Infrastructure;
using UI.GameplayWindow;

namespace Services.StateMachine
{
    public class SetupLevelState : IExitableState
    {
        private readonly WindowsService _windowsService;
        private readonly GridViewService _gridLogicService;
        private readonly GameplayContainer _gameplayContainer;

        public SetupLevelState(WindowsService windowsService, GridViewService gridLogicService, GameplayContainer gameplayContainer)
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
            _gridLogicService.GridView.Enable(true);
            _gameplayContainer.Get<EnemyGrid>().gameObject.SetActive(true);
        }

        public void Exit()
        {
            _gridLogicService.GridView.Enable(false);
            _gameplayContainer.Get<EnemyGrid>().gameObject.SetActive(false);
        }
    }
}