using Gameplay.Units;
using Infrastructure;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public class Platform : MonoBehaviour
    {
        public Vector2Int Index { get; private set; }
        public Actor Actor { get; set; }
        public bool Busy => Actor;
        public bool Free => !Actor;

        private GridViewService _viewService;

        public void Init(int i, int j)
        {
            _viewService = ServiceLocator.Resolve<GridViewService>();
            Index = new Vector2Int(i, j);
        }

        public void Clear()
        {
            Actor.Dispose();
            Destroy(Actor.gameObject);
            Actor = null;
        }

        public void OnMouseDown() => _viewService.OnClicked(this);
        private void OnMouseUp() => _viewService.OnReleased(this);
        private void OnMouseEnter() => _viewService.OnHovered(this);
    }
}