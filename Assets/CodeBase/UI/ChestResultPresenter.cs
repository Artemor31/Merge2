using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChestResultPresenter : Presenter
    {
        [SerializeField] private Button _ok;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _race;
        [SerializeField] private Image _mastery;

        public override void Init() => _ok.onClick.AddListener(Ok);
        private void Ok() => gameObject.SetActive(false);
        public void SetText(string openContent) => _text.text = openContent;
        public void SetRace(Sprite sprite) => _race.sprite = sprite;
        public void SetMastery(Sprite sprite) => _mastery.sprite = sprite;
    }
}