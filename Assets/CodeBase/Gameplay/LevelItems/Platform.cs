using Gameplay.Units;
using Services;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class Platform : MonoBehaviour
    {
        public Vector2Int Index { get; private set; }
        public Actor Actor { get; set; }
        public bool Busy => Actor;
        public bool Free => !Actor;
        private GridViewService _viewService;
        
        public void Init(GridViewService viewService, int i, int j)
        {
            _viewService = viewService;
            Index = new Vector2Int(i, j);
        }

        public void OnMouseDown() => _viewService.OnClicked(this);
        private void OnMouseUp() => _viewService.OnReleased(this);
        private void OnMouseEnter() => _viewService.OnHovered(this);
    }
}