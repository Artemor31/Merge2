using CodeBase.Databases;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class UnitCard : MonoBehaviour
    {
        [field:SerializeField] public Button Button { get; private set; }
        public int Cost { get; private set; }

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _cost;

        public void Setup(UnitsDatabase.UnitData unitData)
        {
            _icon.sprite = unitData.Icon;
            _title.text = unitData.Name;
            Cost = unitData.Cost;
            _cost.text = Cost.ToString();
        }
    }
}