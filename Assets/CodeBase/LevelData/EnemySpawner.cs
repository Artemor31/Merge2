using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class EnemySpawner : LevelItem
    {
        public IReadOnlyList<Vector3> EnemyPositions => _enemyPositions;
        [SerializeField] private List<Vector3> _enemyPositions;

        [SerializeField] private float _delta;
        [SerializeField] private int _countX;
        [SerializeField] private int _countZ;
        
        public void Init()
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
            if (EnemyPositions is {Count: > 0})
            {
                foreach (var position in _enemyPositions)
                {
                    Gizmos.DrawWireSphere(position, _delta / 2);
                }
            }
        }
    }
}