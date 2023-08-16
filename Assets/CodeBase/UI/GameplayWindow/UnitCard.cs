using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class UnitCard : MonoBehaviour
    {
        [field:SerializeField] public Button Button { get; private set; }
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;

        private GameObject _cube;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetTitle(string title)
        {
          //  _title.text = title;
        }
    }
}