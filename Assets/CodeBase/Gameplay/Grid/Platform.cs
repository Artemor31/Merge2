using System;
using Gameplay.Units;
using Infrastructure;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public sealed class Platform : MonoBehaviour
    {
        public int Index { get; private set; }
        public Actor Actor { get; set; }
        public bool Busy => Actor;
        public bool Free => !Actor;

        [SerializeField] private SpriteRenderer _mainRenderer;
        [SerializeField] private SpriteRenderer _lineShowRenderer;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _secondColor;
        [SerializeField] private float _colorChangeSpeed;

        private GridViewService _viewService;
        private ViewState _state;

        public void Init(int index)
        {
            _viewService = ServiceLocator.Resolve<GridViewService>();
            Index = index;
            _state = ViewState.Normal;
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
            UpdateState();
        }

        private void UpdateState()
        {
            _lineShowRenderer.gameObject.SetActive(_state == ViewState.ShowAttackLine);
            _mainRenderer.gameObject.SetActive(_state == ViewState.Normal);
        }

        private void Update()
        {
            if (_state == ViewState.ShowSame)
            {
                float t = Mathf.PingPong(Time.time / 1, _colorChangeSpeed);
                _mainRenderer.color = Color.Lerp(_startColor, _secondColor, t);
            }
        }

        private void OnMouseDown() => _viewService.OnClicked(this);
        private void OnMouseUp() => _viewService.OnReleased(this);
        private void OnMouseEnter() => _viewService.OnHovered(this);
    }

    public enum ViewState
    {
        None = 0,
        Normal = 1,
        ShowSame = 2,
        ShowAttackLine = 3,
        ShowSelected = 4
    }
}