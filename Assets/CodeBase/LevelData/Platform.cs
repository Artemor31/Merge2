using System;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class Platform : MonoBehaviour
    {
        public event Action<Vector2Int> OnClicked;
        public event Action<Vector2Int> OnReleased;
        public event Action<Vector2Int> OnHovered;
        public Vector2Int Index;

        public void OnMouseDown()
        {
            OnClicked?.Invoke(Index);
        }

        private void OnMouseUp()
        {
            OnReleased?.Invoke(Index);
        }

        private void OnMouseEnter()
        {
            OnHovered?.Invoke(Index);
        }
    }
}