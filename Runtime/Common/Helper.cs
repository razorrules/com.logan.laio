using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Laio
{

    public static class Helper
    {

        private static Camera _camera;

        /// <summary>
        /// Static reference to the first camera found. Caches value so it does not look for it every time. 
        /// </summary>
        public static Camera camera
        {
            get
            {
                if (_camera == null) _camera = UnityEngine.Camera.main;
                return _camera;
            }
        }

        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;

        /// <summary>
        /// Is the mouse pointer currently overtop of a UI element.
        /// </summary>
        /// <returns>Is the moust over UI</returns>
        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        /// <summary>
        /// Checks if the game has a save directory
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        internal static bool HasSaveDirectory(string dir = "/Save")
        {
            return Directory.Exists(Application.persistentDataPath + dir);
        }

        /// <summary>
        /// Get a random vector3 that -1,1 not normalized
        /// </summary>
        public static Vector3 RandomVector3(bool randomX = true, bool randomY = true, bool randomZ = true)
        {
            System.Random random = new System.Random();
            Vector3 returnValue = Vector3.zero;
            if (randomX)
                returnValue.x = (float)(random.NextDouble() - .5f) * 2;
            if (randomY)
                returnValue.y = (float)(random.NextDouble() - .5f) * 2;
            if (randomZ)
                returnValue.z = (float)(random.NextDouble() - .5f) * 2;
            return returnValue;
        }

        /// <summary>
        /// Get a random vector3 that -1,1 not normalized
        /// </summary>
        public static Vector3 RandomVector3()
        {
            System.Random random = new System.Random();
            Vector3 returnValue = Vector3.zero;
            returnValue.x = (float)(random.NextDouble() - .5f) * 2;
            returnValue.y = (float)(random.NextDouble() - .5f) * 2;
            returnValue.z = (float)(random.NextDouble() - .5f) * 2;
            return returnValue;
        }

        /// <summary>
        /// Get a random vector3 that -1,1 not normalized
        /// </summary>
        public static Vector2 RandomVector2()
        {
            System.Random random = new System.Random();
            Vector2 returnValue = Vector3.zero;
            returnValue.x = (float)(random.NextDouble() - .5f) * 2;
            returnValue.y = (float)(random.NextDouble() - .5f) * 2;
            return returnValue;
        }

        /// <summary>
        /// Get a random enum value
        /// </summary>
        /// <typeparam name="T">Type of get a rando</typeparam>
        /// <returns></returns>
        public static T RandomEnum<T>() where T : Enum
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(UnityEngine.Random.Range(0, v.Length));
        }
    }
}
