using Databases;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TutorText : Presenter
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TutorTexts _database;

        public void Hide() => _view.SetActive(false);

        public void ShowText(int id)
        {
            _text.text = _database.GetData(id).RusText;
            _view.SetActive(true);
        }
    }
}