using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Laio
{

    public static class Helper
    {

        private static Random Random = new System.Random();

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
        public static bool IsPointerOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        public static Vector3 IntersectionZ(Vector3 origin, Vector3 direction, float targetZ)
        {
            Vector3 value = origin;
            float multiplication = -((origin.z - targetZ) / direction.z);
            value += direction * multiplication;
            return value;
        }

        public static Vector3 IntersectionX(Vector3 origin, Vector3 direction, float targetX)
        {
            Vector3 value = origin;
            float multiplication = -((origin.x - targetX) / direction.x);
            value += direction * multiplication;
            return value;
        }

        public static Vector3 IntersectionY(Vector3 origin, Vector3 direction, float targetY)
        {
            Vector3 value = origin;
            float multiplication = -((origin.y - targetY) / direction.y);
            value += direction * multiplication;
            return value;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static T Next<T>(T current) where T : Enum
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(current.GetType());
            int j = Array.IndexOf<T>(Arr, current) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
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

        public static T GetEnumAtIndex<T>(int index) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), Enum.GetNames(typeof(T))[index]);
        }

        /// <summary>
        /// Get a random enum value excluding elements of such enum.
        /// If calling multiple times, it is recommended to pass the values of the enum, see overload.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="Excluding">Which enum values should we exclude?</param>
        /// <returns></returns>
        public static T RandomEnumExcluding<T>(params T[] Excluding) where T : System.Enum
        {
            T[] values = Enum.GetValues(typeof(T)) as T[];
            if (Excluding.Length >= values.Length)
                throw new IndexOutOfRangeException("Failed to get RandomEnumValueExcluding. Every value was excluded.");
            return values.Except(Excluding).ToArray().Rand();
        }

        /// <summary>
        /// Get a random enum value excluding elements of such enum.
        /// <example>
        /// If you are calling this multiple times, it would be best pratice to cache the values and pass
        /// <code>
        /// myEnum = Enum.GetValues(typeof(MyEnum)) as MyEnum[];
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachedValues">List of all values in a given enum</param>
        /// <param name="Excluding"></param>
        /// <returns></returns>
        public static T RandomEnumExcluding<T>(T[] cachedValues, params T[] Excluding) where T : System.Enum
        {
            if (Excluding.Length >= cachedValues.Length)
                throw new IndexOutOfRangeException("Failed to get RandomEnumValueExcluding. Every value was excluded.");
            return cachedValues.Except(Excluding).ToList().Rand();
        }
    }
}
