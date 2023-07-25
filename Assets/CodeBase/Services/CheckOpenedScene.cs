using CodeBase.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services
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