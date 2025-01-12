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
        public RectTransform RectTransform { get; private set; }
        [SerializeField] private RectTransform _rankParent;
        [SerializeField] private RectTransform _healthParent;

        public override void Init() => RectTransform = GetComponent<RectTransform>();
        public void SetData(GameCanvasData data) => RectTransform = data.IsIdle ? _rankParent : _healthParent;
    }
}