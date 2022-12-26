using UnityEngine.UI;
using UnityEngine;

namespace Laio
{
    /// <summary>
    /// Auto scrolls a raw image on the canvas.
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class ImageScroller : MonoBehaviour
    {
        private RawImage _rawImage;
        [SerializeField] private Vector2 _scrollSpeed;

        private void OnEnable() =>
            _rawImage = GetComponent<RawImage>();

        private void Update() =>
            _rawImage.uvRect = new Rect(_rawImage.uvRect.position + _scrollSpeed * Time.deltaTime, _rawImage.uvRect.size);

    }

}