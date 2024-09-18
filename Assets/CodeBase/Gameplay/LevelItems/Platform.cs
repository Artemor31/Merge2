using System;
using Gameplay.Units;
using Services;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class Platform : MonoBehaviour
    {
        public event Action<Platform> OnClicked;
        public event Action<Platform> OnReleased;
        public event Action<Platform> OnHovered;
        public Vector2Int Index { get; private set; }
        public Actor Actor;
        public bool Busy => Actor != null;
        public bool Free => Actor == null;
        private GridViewService _viewService;
        
        public void Init(GridViewService viewService, int i, int j)
        {
            _viewService = viewService;
            Index = new Vector2Int(i, j);
        }

        public void OnMouseDown()
        {
            _viewService.OnClicked(this);
            OnClicked?.Invoke(this);
        }

        private void OnMouseUp()
        {
            _viewService.OnReleased(this);
            OnReleased?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            _viewService.OnHovered(this);
            OnHovered?.Invoke(this);
        }
    }
}