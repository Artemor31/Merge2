using System;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    [Serializable]
    public class PlayerProgress
    {
        public int Wave;
        public int Money;
        
        public PlayerProgress()
        {
            Wave = PlayerPrefs.GetInt("level", 0);
            Money = PlayerPrefs.GetInt("money", 100);
        }

        public void Save()
        {
            PlayerPrefs.SetInt("level", Wave);
            PlayerPrefs.SetInt("money", Money);
        }
    }
}