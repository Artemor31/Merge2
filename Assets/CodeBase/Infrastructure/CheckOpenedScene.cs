using CodeBase.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
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