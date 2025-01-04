using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoItemPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoActorPresenter _prefab;
        private List<InfoActorPresenter> _presenters = new();
        
        public void SetData(BuffConfig raceConfig, Race race, Dictionary<Mastery,BuffConfig> actors)
        {
            _icon.sprite = raceConfig.Icon;
            _name.text = raceConfig.Name;
            _description.text = raceConfig.Description;
            
            foreach (var actor in actors)
            {
                InfoActorPresenter presenter = Instantiate(_prefab, _actorsParent);

                if (ServiceLocator.Resolve<PersistantDataService>().IsOpened(actor.Key, race))
                {
                    presenter.SetData(actor.Value.Icon, actor.Value.Name);
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