using System;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class CheckOpenedScene : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                ServiceLocator.Resolve<SceneLoader>();
            }
            catch (Exception)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}