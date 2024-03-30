using System;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    [Serializable]
    public class PlayerProgress
    {
        public int Wave;
        public Vector2Int GridSize;
    }
}