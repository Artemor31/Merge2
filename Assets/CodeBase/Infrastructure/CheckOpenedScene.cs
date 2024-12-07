using Services;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class CheckOpenedScene : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceLocator.Resolve<SceneLoader>() == null)
                SceneManager.LoadScene(0);
        }
    }
}