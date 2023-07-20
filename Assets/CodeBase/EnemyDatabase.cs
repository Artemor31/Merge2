using UnityEngine;

namespace CodeBase
{
    [CreateAssetMenu(menuName = "Create EnemyDatabase", fileName = "EnemyDatabase", order = 0)]
    public class EnemyDatabase : ScriptableObject
    {
        public Enemy VariantOne;
    }
}