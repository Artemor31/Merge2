using System;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    [Serializable]
    public class PlayerProgress
    {
        public int Wave;
        public Vector2Int GridSize;
        public int Money;
        
        public PlayerProgress()
        {
            GridSize = new(3, 5);
            Wave = PlayerPrefs.GetInt("level", 0);
            Money = PlayerPrefs.GetInt("money", 100);
        }
    }
}