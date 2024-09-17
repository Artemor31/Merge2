using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class Platform : MonoBehaviour
    {
        public event Action<Vector2Int> OnClicked;
        public event Action<Vector2Int> OnReleased;
        public event Action<Vector2Int> OnHovered;
        public Vector2Int Index { get; private set; }
        public Actor Actor;
        public bool Busy => Actor != null;
        public bool Free => Actor == null;


        public void Init(int i, int j) => Index = new Vector2Int(i, j);
        public void OnMouseDown() => OnClicked?.Invoke(Index);
        private void OnMouseUp() => OnReleased?.Invoke(Index);
        private void OnMouseEnter() => OnHovered?.Invoke(Index);
    }
}