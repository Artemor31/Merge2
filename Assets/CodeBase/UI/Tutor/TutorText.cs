using Databases;
using TMPro;
using UnityEngine;

namespace UI
{

    public enum TextAllignment
    {
        Center,
        Top,
        Bottom
    }
    public class TutorText : Presenter
    {
        [SerializeField] private RectTransform _view;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TutorTexts _database;

        public void Hide() => _view.gameObject.SetActive(false);

        public void ShowText(int id, TextAllignment alignment = TextAllignment.Center)
        {
            _text.text = _database.GetData(id).RusText;
            _view.gameObject.SetActive(true);
            MovePanel(alignment);            
            ResizeDarkPanel(id);
        }

        private void MovePanel(TextAllignment alignment)
        {
            switch (alignment)
            {
                case TextAllignment.Top:
                    _view.anchorMax = new Vector2(0.5f, 1f);
                    _view.anchorMin = new Vector2(0.5f, 1f);
                    _view.anchoredPosition = new Vector2(0, 800);
                    break;
                case TextAllignment.Center:
                    _view.anchorMax = new Vector2(0.5f, 0.5f);
                    _view.anchorMin = new Vector2(0.5f, 0.5f);
                    _view.anchoredPosition = new Vector2(0, 10);
                    break;
                case TextAllignment.Bottom:
                    _view.anchorMax = new Vector2(0.5f, 0f);
                    _view.anchorMin = new Vector2(0.5f, 0f);
                    _view.anchoredPosition = new Vector2(0, -800);
                    break;
            }
        }

        private void ResizeDarkPanel(int id)
        {
            Vector2 textSize = _text.GetPreferredValues();
            _view.sizeDelta = textSize;

            if (id == 0)
            {
                _view.sizeDelta = new Vector2(800, 90);
            }
        }
    }
}