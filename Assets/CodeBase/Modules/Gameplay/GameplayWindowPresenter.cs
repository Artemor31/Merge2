namespace CodeBase.Modules.Gameplay
{
    public class GameplayWindowPresenter
    {
        private readonly GameplayModel _model;
        private readonly GameplayWindow _window;

        public GameplayWindowPresenter(GameplayModel model, GameplayWindow window)
        {
            _model = model;
            _window = window;
        }
    }
}