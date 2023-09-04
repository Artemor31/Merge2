using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class LevelStaticData : MonoBehaviour, IModel
    {
        [SerializeField] private Vector3 _enemyStartPosition;
        [SerializeField] private Vector3 _playerStartPosition;
        [SerializeField] private float _delta;
        [SerializeField] private int _countX;
        [SerializeField] private int _countZ;

        [SerializeField] private List<Vector3> _enemyPositions;
        [SerializeField] private List<Vector3> _playerPositions;

        public IReadOnlyList<Vector3> EnemyPositions => _enemyPositions;
        public IReadOnlyList<Vector3> PlayerPositions => _playerPositions;

        private void Awake() => 
            ModelsContainer.Bind(this);

        [Button]
        public void FillPositions()
        {
            _enemyPositions = FillPositionsFor(_enemyStartPosition);
            _playerPositions = FillPositionsFor(_playerStartPosition);
        }

        private List<Vector3> FillPositionsFor(Vector3 startPosition)
        {
            var positions = new List<Vector3>();
            for (int i = 0; i < _countX; i++)
            {
                float currentX = startPosition.x + _delta * i;

                for (int j = 0; j < _countZ; j++)
                {
                    float currentZ = startPosition.z + _delta * j;
                    positions.Add(new Vector3(currentX, 0, currentZ));
                }
            }

            return positions;
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
            if (_playerPositions != null && _playerPositions.Count > 0)
            {
                foreach (var position in _playerPositions)
                {
                    Gizmos.DrawWireSphere(position, _delta / 2);
                }
            }
        }
    }
}