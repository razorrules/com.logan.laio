using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Laio
{

    public delegate void OnProgressUpdate(float progress);
    public delegate void OnLoadingScreenVisibilityChange(bool isVisible);
    public delegate void OnLoadingFinished();

    /// <summary>
    /// Static class to handle loading screens in unity.
    /// Use delegates to get information on changes, and simply call LoadScene.
    /// 
    /// When loading the scene, there will be a couple frames where the scene is loaded
    /// but the screen does not disable. This is to ensure that the progress bar will 
    /// always fill up, even if it requires extra time after the scene is done loading.
    /// </summary>
    public static class SceneLoading
    {
        public static OnProgressUpdate onProgressUpdate;
        public static OnLoadingScreenVisibilityChange onLoadignScreenVisibilityChange;
        public static OnLoadingFinished onLoadingFinished;

        internal static AsyncOperation asyncOperation;
        internal static float targetValue;
        internal static float currentValue;

        /// <summary>
        /// Load scene and begin the loading screen delegates.
        /// </summary>
        /// <param name="buildIndex">Build index to load</param>
        public static void LoadScene(int buildIndex)
        {
            asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
            onLoadignScreenVisibilityChange?.Invoke(true);
            Loading();
        }

        /// <summary>
        /// Load scene and begin the loading screen delegates.
        /// </summary>
        /// <param name="buildIndex">Scene name.</param>
        public static void LoadScene(string sceneName)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            onLoadignScreenVisibilityChange?.Invoke(true);
            Loading();
        }

        /// <summary>
        /// Loads scene. Use this if you want to handle loading scenes elsewhere, simply pass
        /// the AsyncOperation that SceneManager returns.
        /// </summary>
        /// <param name="loadSceneOperation">AsyncOperation returned from SceneManager.LoadScene()</param>
        public static void LoadScene(AsyncOperation loadSceneOperation)
        {
            asyncOperation = loadSceneOperation;
            onLoadignScreenVisibilityChange?.Invoke(true);
            Loading();
        }

        /// <summary>
        /// Async method for updating loading progress bar. 
        /// </summary>
        private static async void Loading()
        {
            currentValue = 0.0f;

            do
            {

                targetValue = asyncOperation.progress;

                //If you quit playing during loading, break out of the loop so this will never run in the background.
                if (!Application.isPlaying)
                    break;

                currentValue = Mathf.MoveTowards(currentValue, targetValue, 1 * Time.deltaTime);
                onProgressUpdate?.Invoke(currentValue);

                await Task.Yield();

            } while (currentValue < 1);

            asyncOperation = null;
            onLoadingFinished?.Invoke();
            onLoadignScreenVisibilityChange?.Invoke(false);
        }


    }
}