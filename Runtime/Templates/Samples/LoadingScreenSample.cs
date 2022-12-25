using UnityEngine;
using UnityEngine.UI;

namespace Laio.Samples
{

    /// <summary>
    /// Sample class that uses SceneLoading. Singleton that does not destroy on load.
    /// Easily extensible to add additional features such as changing background images, loading bar style, etc.
    /// </summary>

    public class LoadingScreenSample : Singleton<LoadingScreenSample>
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private Slider _slider;

        /// <summary>
        /// Setup singleton along with assining methods to correct delegates.
        /// </summary>
        private void Awake()
        {
            SetInstance(this, SingletonParams.DontDestroyOnLoad);
            SceneLoading.onProgressUpdate += OnProgressUpdate;
            SceneLoading.onLoadignScreenVisibilityChange += OnLoadingScreenVisibilityChange;
        }

        /// <summary>
        /// Remove all delegates on destroy
        /// </summary>
        private void OnDestroy()
        {
            SceneLoading.onProgressUpdate -= OnProgressUpdate;
            SceneLoading.onLoadignScreenVisibilityChange -= OnLoadingScreenVisibilityChange;
        }

        /// <summary>
        /// Called when the loading screen should change visibility
        /// </summary>
        /// <param name="isVisible">Should it be visible</param>
        public void OnLoadingScreenVisibilityChange(bool isVisible)
        {
            _loadingPanel.SetActive(isVisible);
        }

        /// <summary>
        /// Called when progress updates
        /// </summary>
        /// <param name="progress">Current progress of loading. (0-1)</param>
        public void OnProgressUpdate(float progress)
        {
            _slider.value = progress;
        }

    }

}