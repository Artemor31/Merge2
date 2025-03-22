using System;

namespace Services.ProgressData
{
    [Serializable]
    public class GameplayProgress : SaveData
    {
        public int Crowns = 20;
        public int Wave;
    }
}