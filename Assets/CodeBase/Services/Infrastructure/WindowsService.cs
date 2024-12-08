using System.Collections.Generic;
using System.Linq;
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

        public void Show<T>() where T : Presenter
        {
            _windows.First(w => w.GetType() == typeof(T))
                    .gameObject
                    .SetActive(true);
        }

        public void Close<T>()
        {
            _windows.First(w => w.GetType() == typeof(T))
                    .gameObject
                    .SetActive(false);
        }
    }
}