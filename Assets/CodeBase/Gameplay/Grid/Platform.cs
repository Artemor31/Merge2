using Gameplay.Units;
using Infrastructure;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public class Platform : MonoBehaviour
    {
        public Collider Collider;
        public MeshRenderer SameUnitView;
        public int Index { get; private set; }
        public Actor Actor { get; set; }
        public bool Busy => Actor;
        public bool Free => !Actor;
        protected GridViewService _viewService;
        
        public virtual void Init(int index)
        {
            _viewService = ServiceLocator.Resolve<GridViewService>();
            Index = index;
        }

        public void Clear()
        {
            Actor.Dispose();
            Destroy(Actor.gameObject);
            Actor = null;
        }

        private void OnMouseDown() => _viewService.OnClicked(this);
        private void OnMouseUp() => _viewService.OnReleased(this);
        private void OnMouseEnter() => _viewService.OnHovered(this);
    }
}