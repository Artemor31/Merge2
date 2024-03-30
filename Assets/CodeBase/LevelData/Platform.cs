using System;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class Platform : LevelItem
    {
        public event Action<Platform> OnClicked;
        public event Action<Platform> OnReleased;
        public event Action<Vector2Int> OnHovered;
        public Vector2Int Index;

        public Platform Init(int i, int j)
        {
            Index = new Vector2Int(i, j);
            return this;
        }

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