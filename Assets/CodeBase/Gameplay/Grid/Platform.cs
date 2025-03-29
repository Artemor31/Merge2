using Databases;
using Gameplay.Units;
using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay.Grid
{
    public class StashPlatform : Platform
    {
        
    }
    
    public class Platform : MonoBehaviour
    {
        public ActorData Data => Actor == null ? default : Actor.Data;
        public int Index { get; private set; }
        public Actor Actor { get; set; }
        public bool Busy => Actor;
        public bool Free => !Actor;

        [SerializeField] private SpriteRenderer _mainRenderer;
        [SerializeField] private SpriteRenderer _lineShowRenderer;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _secondColor;
        [SerializeField] private float _colorChangeSpeed;

        private GridService _service;
        private ViewState _state;

        public void Init(int index)
        {
            _service = ServiceLocator.Resolve<GridService>();
            Index = index;
            _state = ViewState.Normal;
            gameObject.SetActive(true);
            SetViewState(ViewState.Normal);
        }

        public void Clear()
        {
            Actor.Dispose();
            Destroy(Actor.gameObject);
            Actor = null;
        }

        public void SetViewState(ViewState state)
        {
            _state = state;
            _lineShowRenderer.gameObject.SetActive(_state == ViewState.ShowAttackLine);
            _mainRenderer.gameObject.SetActive(_state is ViewState.Normal or ViewState.ShowSame);

            _mainRenderer.color = _state switch
            {
                ViewState.Normal => Color.white,
                ViewState.ShowSame => Color.green,
                _ => _mainRenderer.color
            };
        }

        private void Update()
        {
            if (_state == ViewState.ShowAttackLine)
            {
                float t = Mathf.PingPong(Time.time / 1, _colorChangeSpeed);
                _lineShowRenderer.color = Color.Lerp(_startColor, _secondColor, t);
            }
        }

        private void OnMouseDown() => _service.OnMouseDown(this);
        private void OnMouseUp() => _service.OnMouseUp(this);
        private void OnMouseEnter() => _service.OnMouseEnter(this);
    }

    public enum ViewState
    {
        None = 0,
        Normal = 1,
        ShowSame = 2,
        ShowAttackLine = 3,
        ShowSelected = 4,
        Disabled = 5
    }
}