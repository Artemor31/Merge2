using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class GridView : MonoBehaviour
    {
        private static readonly int SelectCell = Shader.PropertyToID("_SelectCell");
        private static readonly int SelectedCellX = Shader.PropertyToID("_SelectedCellX");
        private static readonly int SelectedCellY = Shader.PropertyToID("_SelectedCellY");

        [SerializeField] private Material _material;
        private GridViewService _gridViewService;

        public void Init()
        {
            _material.SetFloat(SelectCell, 0);
            _gridViewService = ServiceLocator.Resolve<GridViewService>();
            _gridViewService.OnPlatformClicked += PlatformOnOnClicked;
            _gridViewService.OnPlatformHovered += PlatformOnOnHovered;
            _gridViewService.OnPlatformReleased += PlatformOnOnReleased;
        }

        private void PlatformOnOnClicked(Platform gridData)
        {
            SetSelected(gridData.Index);
            SetHighlighted(true);
        }

        private void PlatformOnOnReleased(Platform gridData) => SetHighlighted(false);
        private void PlatformOnOnHovered(Platform gridData) => SetSelected(gridData.Index);
        private void SetHighlighted(bool highlight) => _material.SetFloat(SelectCell, highlight ? 1 : 0);
        private void SetSelected(Vector2Int position)
        {
            _material.SetFloat(SelectedCellX, position.y);
            _material.SetFloat(SelectedCellY, position.x);
        }
    }
}