using UnityEngine;

namespace UI.GameplayWindow
{
    public class GameCanvasData : WindowData
    {
        public readonly bool IsIdle;
        public GameCanvasData(bool isIdle) => IsIdle = isIdle;
    }
    
    public class GameCanvas : Presenter, IWindowDataReceiver<GameCanvasData>
    {
        public RectTransform RankParent => _rankParent;
        public RectTransform HealthParent => _healthParent;
        [SerializeField] private RectTransform _rankParent;
        [SerializeField] private RectTransform _healthParent;

        public void SetData(GameCanvasData data)
        {
            _rankParent.gameObject.SetActive(data.IsIdle);
            _healthParent.gameObject.SetActive(!data.IsIdle);
        }
    }
}