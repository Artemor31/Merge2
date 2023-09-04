using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.Utility;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Test
{
    public class ProgressBarTest : MonoBehaviour
    {
        [Header("Constant ProgressBar")]
        [ProgressBar("Health", 100, EColor.Red)]
        public float health = 50.0f;

        [Header("Nested ProgressBar")]
        public ProgressBarNest1 nest1;

        [Header("Dynamic ProgressBar")]
        [ProgressBar("Elixir", "maxElixir", color: EColor.Violet)]
        public int elixir = 50;
        public int maxElixir = 100;
    }

    [System.Serializable]
    public class ProgressBarNest1
    {
        [ProgressBar("Mana", 100, EColor.Blue)]
        public float mana = 25.0f;

        public ProgressBarNest2 nest2;
    }

    [System.Serializable]
    public class ProgressBarNest2
    {
        [ProgressBar("Stamina", 100, EColor.Green)]
        public float stamina = 75.0f;
    }
}
