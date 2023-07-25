using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class UnitCard : MonoBehaviour, IPointerClickHandler
    {
        public event Action<UnitCard> Clicked; 
        
        [SerializeField] private Image _icon;

        private bool _clicked;
        private GameObject _cube;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this);
        }
    }
}