using System.Collections.Generic;
using Databases;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeWindow
{
    public class InfoItemPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoActorPresenter _prefab;

        private List<InfoActorPresenter> _presenters = new();

        public void SetData(Sprite sprite, string name, string description, Dictionary<Mastery, BuffConfig> actors)
        {
            _icon.sprite = sprite;
            _name.text = name;
            _description.text = description;

            foreach (var actor in actors)
            {
                InfoActorPresenter presenter = Instantiate(_prefab, _actorsParent);
                presenter.SetData(actor.Value.Icon, actor.Value.Name);
                _presenters.Add(presenter);
            }
        }
    }
}