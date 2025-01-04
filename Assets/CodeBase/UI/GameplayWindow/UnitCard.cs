using Databases;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class UnitCard : MonoBehaviour
    {
        [field:SerializeField] public Button Button { get; private set; }
        public int Cost { get; private set; }

        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _cost;

        public void Setup(ActorConfig unitData)
        {
            _title.text = unitData.Data.Mastery.ToString() + unitData.Data.Race + unitData.Data.Level;
            Cost = unitData.Cost;
            _cost.text = Cost.ToString();
        }
    }
}