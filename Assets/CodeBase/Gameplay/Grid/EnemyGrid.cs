using System.Collections.Generic;
using UnityEngine;
using System;
using Infrastructure;
using Services;

namespace Gameplay.Grid
{
    public class EnemyGrid : MonoBehaviour
    {
        [SerializeField] private List<GridLine> _lines;

        private void Start() => ServiceLocator.Resolve<GameplayContainer>().Add(this);

        public void Disable() => _lines.ForEach(l => l.Disable());

        public void Highlinght(int line)
        {
            Disable();
            _lines[line].Highlinght();
        }

        [Serializable]
        private class GridLine
        {
            [SerializeField] private List<GameObject> _objects;
            public void Highlinght() => _objects.ForEach(obj => obj.SetActive(true));
            public void Disable() => _objects.ForEach(obj => obj.SetActive(false));
        }
    }
}