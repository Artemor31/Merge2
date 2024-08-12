using Services.SaveService;
using UnityEngine;

namespace LevelData
{
    public class GridView : MonoBehaviour
    {
        private static readonly int SelectCell = Shader.PropertyToID("_SelectCell");
        private static readonly int SelectedCellX = Shader.PropertyToID("_SelectedCellX");
        private static readonly int SelectedCellY = Shader.PropertyToID("_SelectedCellY");

        [SerializeField] private Material _material;
        private GridService _gridService;

        public void Init(GridService gridService)
        {
            _gridService = gridService;
            _material.SetFloat(SelectCell, 0);
            _gridService.OnPlatformClicked += PlatformOnOnClicked;
            _gridService.OnPlatformHovered += PlatformOnOnHovered;
            _gridService.OnPlatformReleased += PlatformOnOnReleased;
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