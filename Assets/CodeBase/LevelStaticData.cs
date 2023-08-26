using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase
{
    public class LevelStaticData : MonoBehaviour, IModel
    {
        [SerializeField] private List<Vector3> _enemyPositions;
        [SerializeField] private List<Vector3> _playerPositions;

        public IReadOnlyList<Vector3> EnemyPositions => _enemyPositions;
        public IReadOnlyList<Vector3> PlayerPositions => _playerPositions;

        private void Awake()
        {
            ModelsContainer.Bind(this);
        }
    }
}