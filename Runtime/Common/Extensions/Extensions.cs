using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using Laio;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

/*
 * Code Ripped:
 * https://github.com/mikecann/Unity-Helpers
 * 
 * Look into and improve AddChild
 * maybe <T params>
 * 
 */


public static class Extensions
{

    //--------------- Transforms --------------//

    /// <summary>
    /// Destroy all child game objects of transform.
    /// </summary>
    public static void DestroyAllChildren(this Transform transform)
    { foreach (Transform t in transform) { UnityEngine.Object.Destroy(t.gameObject); } }

    public static T GetComponentRef<T>(this Transform t, out T c)
    {
        c = t.GetComponent<T>();
        return c;
    }

    //TODO: Implement
    public static Vector3 GetDirectionClamped(this Transform t, Vector3 target)
    {
        return Vector3.forward;
    }

    /// <summary>
    /// Immediately destroy all child game objects of transform.
    /// </summary>
    public static void DestroyAllChildrenImmediate(this Transform transform)
    { foreach (Transform t in transform) { UnityEngine.Object.DestroyImmediate(t.gameObject); } }

    public static void MoveChildren(this Transform transform, Transform parent)
    { foreach (Transform t in transform) { t.SetParent(parent); } }

    /// <summary>
    /// A shortcut for creating a new game object then adding a component then adding it to a parent object
    /// </summary>
    /// <typeparam name="T">Type of component</typeparam>
    /// <returns>The new component</returns>
    public static T AddChild<T>(this GameObject parent) where T : Component
    {
        return AddChild<T>(parent, typeof(T).Name);
    }

    /// <summary>
    /// A shortcut for creating a new game object then adding a component then adding it to a parent object
    /// </summary>
    /// <typeparam name="T">Type of component</typeparam>
    /// <param name="name">Name of the child game object</param>
    /// <returns>The new component</returns>
    public static T AddChild<T>(this GameObject parent, string name) where T : Component
    {
        var obj = AddChild(parent, name, typeof(T));
        return obj.GetComponent<T>();
    }

    /// <summary>
    /// A shortcut for adding a given game object as a child
    /// </summary>
    /// <returns>This gameobject</returns>
    public static GameObject AddChild(this GameObject parent, GameObject child, bool worldPositionStays = false)
    {
        child.transform.SetParent(parent.transform, worldPositionStays);
        return parent;
    }


    /// <summary>
    /// A shortcut for creating a new game object with a number of components and adding it as a child
    /// </summary>
    /// <param name="components">A list of component types</param>
    /// <returns>The new game object</returns>
    public static GameObject AddChild(this GameObject parent, params Type[] components)
    {
        return AddChild(parent, "Game Object", components);
    }
    /// <summary>
    /// A shortcut for creating a new game object with a number of components and adding it as a child
    /// </summary>
    /// <param name="name">The name of the new game object</param>
    /// <param name="components">A list of component types</param>
    /// <returns>The new game object</returns>
    public static GameObject AddChild(this GameObject parent, string name, params Type[] components)
    {
        var obj = new GameObject(name, components);
        if (parent != null)
        {
            if (obj.transform is RectTransform) obj.transform.SetParent(parent.transform, true);
            else obj.transform.parent = parent.transform;
        }
        return obj;
    }


    //----------------- List -----------------//

    /// <summary>
    /// Get a random element from List using UnityEngine.Random.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Rand<T>(this IList<T> list)
    { return list[UnityEngine.Random.Range(0, list.Count)]; }


    /// <summary>
    /// Shuffle full will pick two random indexs and swap them n * n times
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void ShuffleFull<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count * list.Count; i++)
        {
            int a = UnityEngine.Random.Range(0, list.Count);
            int b = UnityEngine.Random.Range(0, list.Count);
            list.Swap(a, b);
        }
    }

    /// <summary>
    /// Simple and faster shuffle that will iterate through loop and swap it with random index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void ShuffleFast<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int a = UnityEngine.Random.Range(0, list.Count);
            list.Swap(i, a);
        }
    }

    /// <summary>
    /// Swap elements based on index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        T temp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = temp;
    }

    //----------------- Other -----------------//

    /// <summary>
    /// Saves the scriptable obejct as a JSON to persistent data path.
    /// </summary>
    public static void Save(this ScriptableObject objectToSave, string name, string path = "/Save/", string extension = ".txt")
    {
        if (objectToSave == null)
        {
            Debug.LogError("Attempted to save a null object");
            return;
        }
        if (!Helper.HasSaveDirectory())
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
        }

        string jsonSave = JsonUtility.ToJson(objectToSave);
        using (FileStream file = File.Create(Application.persistentDataPath + path + name + extension))
        {
            byte[] bytes = Encoding.ASCII.GetBytes(jsonSave);
            file.Write(bytes, 0, bytes.Length);
        }

    }

    /// <summary>
    /// Load a scriptable object
    /// </summary>
    public static ScriptableObject LoadObject<T>(string name, string path = "/Save/", string extension = ".txt")
    {
        ScriptableObject returnObj = ScriptableObject.CreateInstance(typeof(T));

        if (!Helper.HasSaveDirectory(path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
            return null;
        }

        if (File.Exists(Application.persistentDataPath + path + name + extension))
        {
            string fileContents;

            FileStream file = File.Open(Application.persistentDataPath + path + name + extension, FileMode.Open);
            using (StreamReader reader = new StreamReader(file))
            {
                fileContents = reader.ReadToEnd();
            }
            file.Close();

            JsonUtility.FromJsonOverwrite(fileContents, returnObj);

            return returnObj;
        }

        Debug.LogWarning("No save detected.");
        return null;
    }

    /// <summary>
    /// Load a scriptable object
    /// </summary>
    public static ScriptableObject LoadObject(Type type, string name, string path = "/Save/", string extension = ".txt")
    {
        ScriptableObject returnObj = ScriptableObject.CreateInstance(type);

        if (!Helper.HasSaveDirectory(path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
            Debug.Log("Path does not exist");
            return null;
        }

        if (File.Exists(Application.persistentDataPath + path + name + extension))
        {
            string fileContents;

            FileStream file = File.Open(Application.persistentDataPath + path + name + extension, FileMode.Open);
            using (StreamReader reader = new StreamReader(file))
            {
                fileContents = reader.ReadToEnd();
            }
            file.Close();

            JsonUtility.FromJsonOverwrite(fileContents, returnObj);

            return returnObj;
        }
        Debug.LogWarning("No save detected.");
        return null;
    }

    public static void SetOrthographicSizeWithWidthLock(this Camera cam, float desiredHeight)
    {
        float desiredAspect = 16f / 9f;
        float aspect = cam.aspect;
        float ratio = desiredAspect / aspect;
        cam.orthographicSize = desiredHeight * ratio;
    }

    /// <summary>
    /// Load a scriptable object
    /// </summary>
    public static void Load(this ScriptableObject obj, string name, string path = "/Save/", string extension = ".txt")
    {
        obj = LoadObject(obj.GetType(), name, path, extension);
    }


    //C:\Users\logan\AppData\LocalLow\Logan\Laio\Save

}