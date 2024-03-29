using System;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class Platform : MonoBehaviour
    {
        public event Action<Platform> OnClicked;
        public event Action<Platform> OnReleased;
        public event Action<Vector2Int> OnHovered;
        public Vector2Int Index;

        public void OnMouseDown()
        {
            OnClicked?.Invoke(this);
        }

        private void OnMouseUp()
        {
            OnReleased?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            OnHovered?.Invoke(Index);
        }
    }
}