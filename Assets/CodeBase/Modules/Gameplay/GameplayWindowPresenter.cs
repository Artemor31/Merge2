namespace CodeBase.Modules.Gameplay
{
    public class GameplayWindowPresenter
    {
        private readonly IGameplayModel _model;
        private readonly GameplayWindow _window;

        public GameplayWindowPresenter(IGameplayModel model, GameplayWindow window)
        {
            _model = model;
            _window = window;
        }
    }
}