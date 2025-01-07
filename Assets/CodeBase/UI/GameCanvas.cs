using UnityEngine;

namespace UI
{
    public class GameCanvas : Presenter
    {
        public RectTransform RectTransform { get; private set; }
        public override void Init() => RectTransform = GetComponent<RectTransform>();
    }
}