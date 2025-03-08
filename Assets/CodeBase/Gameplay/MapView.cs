using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private GameObject _castle;
        [SerializeField] private GameObject _cementry;
        private void Start()
        {
            int wave = ServiceLocator.Resolve<GameplayDataService>().Wave;
            if (wave is > 14 and < 30)
            {
                _castle.gameObject.SetActive(false);
                _cementry.gameObject.SetActive(true);
            }
            else
            {
                _castle.gameObject.SetActive(true);
                _cementry.gameObject.SetActive(false);
            }
        }
    }
}