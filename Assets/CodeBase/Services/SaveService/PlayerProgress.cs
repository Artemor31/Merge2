using System;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerProgress(int wave, Vector2Int gridSize, int money)
        {
            Wave = wave;
            GridSize = gridSize;
            Money = money;
        }
        public int Wave;
        public Vector2Int GridSize;
        public int Money;
    }
}