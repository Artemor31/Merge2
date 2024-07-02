using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI
{
    public class WindowsService : MonoBehaviour, IService
    {
        [SerializeField] private List<Presenter> _windows;

        public void Show<T>() where T : Presenter
        {
            _windows.First(w => w.GetType() == typeof(T))
                    .gameObject
                    .SetActive(true);
        }

        public void InitWindows()
        {
            foreach (Presenter presenter in _windows)
            {
                presenter.Init();
            }
        }
    }
}