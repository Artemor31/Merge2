using System.Collections.Generic;
using Databases.BuffConfigs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow
{
    public class InfoItemPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoActorPresenter _prefab;
        private readonly List<InfoActorPresenter> _presenters = new();
        
        public void SetData(BuffConfig raceConfig, IEnumerable<(BuffConfig config, bool opened)> actors, bool isOpened)
        {
            _icon.sprite = raceConfig.Icon;

            if (isOpened)
            {
                _name.text = raceConfig.RoleName;
                _description.text = raceConfig.GetDescription();
            }
            else
            {
                _name.text = _description.text = "???";
            }
            
            foreach (var actor in actors)
            {
                InfoActorPresenter presenter = Instantiate(_prefab, _actorsParent);

                if (actor.opened)
                {
                    presenter.SetData(actor.config.Icon, actor.config.RoleName, actor.config.GetDescription());
                }
                else
                {
                    presenter.SetClosed();
                }
                
                _presenters.Add(presenter);
            }
        }
    }
}