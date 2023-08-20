using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase
{
    public class LevelStaticData : MonoBehaviour, IModel
    {
        public List<Vector3> EnemyPositions;
        public List<Vector3> PlayerPositions;

        private void Awake()
        {
            ModelsContainer.Bind(this);
        }
    }
}