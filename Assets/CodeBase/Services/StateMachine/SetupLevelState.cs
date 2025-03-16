using Services.GridService;
using Services.Infrastructure;
using UI.GameplayWindow;

namespace Services.StateMachine
{
    public class SetupLevelState : IExitableState
    {
        private readonly WindowsService _windowsService;
        private readonly GridLogicService _gridLogicService;

        public SetupLevelState(WindowsService windowsService, GridLogicService gridLogicService)
        {
            _windowsService = windowsService;
            _gridLogicService = gridLogicService;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();
            _windowsService.Show<ShopPresenter>();
            _windowsService.Show<GameCanvas, GameCanvasData>(new GameCanvasData(true));
            _gridLogicService.GridView.Enable(true);
        }

        public void Exit() => _gridLogicService.GridView.Enable(false);
    }
}