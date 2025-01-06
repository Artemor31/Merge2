using Databases;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class UnitCard : MonoBehaviour
    {
        [field:SerializeField] public Button Button { get; private set; }
        [SerializeField] private TMP_Text _title;

        public void Setup(ActorConfig unitData)
        {
            _title.text = unitData.Data.Mastery.ToString() + unitData.Data.Race + unitData.Data.Level;
        }
    }
}