using CodeBase.Gameplay;
using CodeBase.Models;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindowPresenter
    {
        private readonly GameplayModel _model;
        private readonly GameplayWindow _window;

        public GameplayWindowPresenter(GameplayModel model, GameplayWindow window)
        {
            _model = model;
            _window = window;
            
            _window.StartWave.onClick.AddListener(WaveStarted);
        }

        private void WaveStarted()
        {
            _model.State = GameState.Processing;
            _window.StartWave.gameObject.SetActive(false);
        }
    }
}