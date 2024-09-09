using Data;
using Services;
using Services.SaveService;
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

        public void Init(GridViewService gridViewService)
        {
            _gridViewService = gridViewService;
            _material.SetFloat(SelectCell, 0);
            _gridViewService.OnPlatformClicked += PlatformOnOnClicked;
            _gridViewService.OnPlatformHovered += PlatformOnOnHovered;
            _gridViewService.OnPlatformReleased += PlatformOnOnReleased;
        }

        private void PlatformOnOnClicked(GridRuntimeData gridData)
        {
            SetSelected(gridData.Index);
            SetHightighted(true);
        }

        private void PlatformOnOnReleased(GridRuntimeData gridData) => SetHightighted(false);
        private void PlatformOnOnHovered(GridRuntimeData gridData) => SetSelected(gridData.Index);
        
        private void SetHightighted(bool highlight) => _material.SetFloat(SelectCell, highlight ? 1 : 0);

        private void SetSelected(Vector2Int position)
        {
            _material.SetFloat(SelectedCellX, position.y);
            _material.SetFloat(SelectedCellY, position.x);
        }
    }
}