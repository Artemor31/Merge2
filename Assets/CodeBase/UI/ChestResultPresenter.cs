using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChestResultPresenter : Presenter
    {
        [SerializeField] private Button _ok;
        [SerializeField] private TextMeshProUGUI _text;

        public override void Init() => _ok.onClick.AddListener(Ok);
        private void Ok() => gameObject.SetActive(false);
        public void SetData(string openContent) => _text.text = openContent;
    }
}