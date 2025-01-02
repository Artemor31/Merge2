using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UI;
using UnityEngine;

namespace Services.Infrastructure
{
    public class WindowsService : MonoBehaviour, IService
    {
        [SerializeField] private List<Presenter> _windows;

        public void InitWindows()
        {
            foreach (Presenter presenter in _windows)
            {
                presenter.Init();
            }
        }

        [Button]
        public void FindPresenters()
        {
            _windows.Clear();
            _windows = transform.GetComponentsInChildren<Presenter>().ToList();
        }

        public void Show<T>() where T : Presenter
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(T));
            presenter.OnShow();
            presenter.gameObject.SetActive(true);
        }

        public void Close<T>()
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(T));
            presenter.OnHide();
            presenter.gameObject.SetActive(false);
        }
    }
}