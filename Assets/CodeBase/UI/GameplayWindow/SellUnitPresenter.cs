using TMPro;
using UnityEngine;

namespace UI.GameplayWindow
{
    public class SellUnitPresenter : Presenter
    {
        [SerializeField] private TMP_Text _cost;

        public void Show(int cost)
        {
            gameObject.SetActive(true);
            _cost.text = cost.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}