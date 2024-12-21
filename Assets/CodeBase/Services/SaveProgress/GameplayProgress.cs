using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class GameplayProgress
    {
        public int Money;
        public int Wave;

        public GameplayProgress()
        {
            Money = 100;
            Wave = 0;
        }
    }
}