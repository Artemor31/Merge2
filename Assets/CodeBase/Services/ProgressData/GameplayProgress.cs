using System;

namespace Services.ProgressData
{
    [Serializable]
    public class GameplayProgress : SaveData
    {
        public int Crowns = 5;
        public int Wave;
    }
}