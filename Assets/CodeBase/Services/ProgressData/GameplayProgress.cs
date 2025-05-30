using System;

namespace Services.ProgressData
{
    [Serializable]
    public class GameplayProgress : SaveData
    {
        public int Coins = 5;
        public int Wave;
    }
}