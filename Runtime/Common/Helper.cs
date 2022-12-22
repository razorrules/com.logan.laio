using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Laio
{
    //Load scene async, return progress

    //Include reference to TaroDev
    public static class Helper
    {
        private static Camera _camera;
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
        public static bool IsOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        public static bool HasSaveDirectory(string dir = "/game_save")
        {
            return Directory.Exists(Application.persistentDataPath + dir);
        }

        //public static async string Download(string url)
        //{
        //    HttpWebRequest request;
        //    string responseText = await Task.Run(() =>
        //    {
        //        try
        //        {
        //            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //            WebResponse response = request.GetResponse();
        //            Stream responseStream = response.GetResponseStream();
        //            return new StreamReader(responseStream).ReadToEnd();
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError("Failed make request");
        //        }
        //    });

        //    return null;
        //}


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


        public static class Enum
        {
            public static T Random<T>()
            {
                var v = System.Enum.GetValues(typeof(T));
                return (T)v.GetValue(UnityEngine.Random.Range(0, v.Length));
            }
        }


    }
}
