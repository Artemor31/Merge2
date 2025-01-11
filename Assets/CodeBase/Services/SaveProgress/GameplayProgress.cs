using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class GameplayProgress : SaveData
    {
        public int Crowns = 20;
        public int Wave;
    }
}