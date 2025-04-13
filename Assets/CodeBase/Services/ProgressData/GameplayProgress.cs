using System;

namespace Services.ProgressData
{
    [Serializable]
    public class GameplayProgress : SaveData
    {
        public int Crowns = 200;
        public int Wave;
    }
}