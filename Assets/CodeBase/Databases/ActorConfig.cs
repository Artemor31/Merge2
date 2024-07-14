using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create ActorConfig", fileName = "ActorConfig", order = 0)]
    public class ActorConfig : ScriptableObject
    {
        public UnitId Id;
        public Sprite Icon;
        public GameObject Prefab;
        public string Name;
        public int Level;
        public int Cost;
    }
}