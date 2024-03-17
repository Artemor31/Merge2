using System.Collections.Generic;
using CodeBase.NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class LevelStaticData : MonoBehaviour
    {
        public IReadOnlyList<Vector3> EnemyPositions => _enemyPositions;
        public IReadOnlyList<Platform> PlayerPositions => _playerPositions;
        [SerializeField] private List<Vector3> _enemyPositions;
        [SerializeField] private List<Platform> _playerPositions;

        [SerializeField] private float _delta;
        [SerializeField] private int _countX;
        [SerializeField] private int _countZ;
        
        [Button]
        public void FillPositions()
        {
            _enemyPositions.Clear();
            var positions = new List<Vector3>();
            for (int i = 0; i < _countX; i++)
            {
                float currentX = transform.position.x + _delta * i;

                for (int j = 0; j < _countZ; j++)
                {
                    float currentZ = transform.position.z + _delta * j;
                    positions.Add(new Vector3(currentX, 0, currentZ));
                }
            }

            _enemyPositions = positions;
        }

        private void OnDrawGizmosSelected()
        {
            if (EnemyPositions != null && EnemyPositions.Count > 0)
            {
                foreach (var position in _enemyPositions)
                {
                    Gizmos.DrawWireSphere(position, _delta / 2);
                }
            }
        }
    }
}