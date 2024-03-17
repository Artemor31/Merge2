using UnityEngine;

namespace CodeBase.LevelData
{
    public class GridView : MonoBehaviour
    {
        private static readonly int GridSizeX = Shader.PropertyToID("_GridSizeX");
        private static readonly int GridSizeY = Shader.PropertyToID("_GridSizeY");
        private static readonly int SelectCell = Shader.PropertyToID("_SelectCell");
        private static readonly int SelectedCellX = Shader.PropertyToID("_SelectedCellX");
        private static readonly int SelectedCellY = Shader.PropertyToID("_SelectedCellY");

        public Material Material;
        private Vector2 _size;
        
        private void Start()
        {
            Material = GetComponent<MeshRenderer>().material;
            Material.SetFloat(SelectCell, 0);
            _size = new(Material.GetFloat(GridSizeX), Material.GetFloat(GridSizeY));
        }

        public void SetSelected(Vector2Int position)
        {
            Material.SetFloat(SelectedCellX, position.x);
            Material.SetFloat(SelectedCellY, position.y);
            Material.SetFloat(SelectCell, 1);
        }

        public void SetSelected(bool highlight)
        {
            Material.SetFloat(SelectCell, highlight ? 1 : 0);
        }
    }
}