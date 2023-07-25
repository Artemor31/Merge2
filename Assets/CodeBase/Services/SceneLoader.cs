using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CodeBase.Services
{
    public class SceneLoader : IService
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public void Unload(string name, Action onUnloaded = null) =>
            _coroutineRunner.StartCoroutine(UnloadScene(name, onUnloaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            SceneManager.LoadSceneAsync(nextScene).completed += _ => onLoaded?.Invoke();
        }

        private IEnumerator UnloadScene(string name, Action onUnloaded)
        {
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(name);

            while (unloadSceneAsync.isDone)
            {
                onUnloaded?.Invoke();
                yield break;
            }

            unloadSceneAsync.completed += x => onUnloaded?.Invoke();
        }
    }
}