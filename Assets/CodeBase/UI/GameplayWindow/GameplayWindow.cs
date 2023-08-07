using System.Collections.Generic;
using CodeBase.Databases;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindow : Window
    {
        [SerializeField] private Transform _unitsParent;
        [SerializeField] private UnitCard _cardPrefab;
        [SerializeField] private UnitsDatabase _unitsDatabase;
        [SerializeField] private LayerMask _layerMask;
        
        public Button StartWave;

        private UnitCard _clickedCard;
        private Camera _camera;
        private Dictionary<UnitCard, GameObject> _cards;
        private GameObject _instanceOfUnit;

        private void Start()
        {
            _cards = new();
            _camera = Camera.main;
            foreach (var unitData in _unitsDatabase.Units)
            {
                var unitCard = Instantiate(_cardPrefab, _unitsParent);
                unitCard.SetIcon(unitData.Icon);
                unitCard.Clicked += UnitCardOnClicked;
                _cards.Add(unitCard, unitData.Prefab);
            }
        }

        private void UnitCardOnClicked(UnitCard card)
        {
            _clickedCard = card;
            _instanceOfUnit = Instantiate(_cards[_clickedCard]);
        }

        private void Update()
        {
            if (_clickedCard == null) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hits, 999f, _layerMask))
            {
                var point = hits.point;
                _instanceOfUnit.transform.position = point;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(_instanceOfUnit, _instanceOfUnit.transform.position, Quaternion.identity);
            }
        }
    }
}