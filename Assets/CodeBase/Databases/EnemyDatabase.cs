using CodeBase.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create EnemyDatabase", fileName = "EnemyDatabase", order = 0)]
    public class EnemyDatabase : ScriptableObject
    {
        public Unit VariantOne;
    }
}