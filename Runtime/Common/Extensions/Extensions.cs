using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using Laio;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class Extensions
{
    /// <summary>
    /// Normalizes a vectory if the magnitube is above 1.0
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 NormalizeIfNeeded(this Vector3 v)
    {
        if (v.magnitude > 1)
            return v.normalized;
        return v;
    }

    //--------------- Transforms --------------//

    /// <summary>
    /// Get a random point in bounds. Does not handle rotation. 
    /// 
    /// TODO: Look into this
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns>Random point in bounds</returns>
    public static Vector3 RandomPointInBounds(this Bounds bounds)
    {
        System.Random random = new System.Random();
        Vector3 newPoint = new Vector3(
        (float)(bounds.extents.x * ((random.NextDouble() - .5f) * 2.0f)),
        (float)(bounds.extents.x * ((random.NextDouble() - .5f) * 2.0f)),
        (float)(bounds.extents.x * ((random.NextDouble() - .5f) * 2.0f))
        );
        return newPoint;
    }

    /// <summary>
    /// Get a the direction to a given point
    /// </summary>
    /// <param name="target">Target to get the direction to</param>
    /// <returns>Direction to target</returns>
    public static Vector3 GetDirection(this Transform t, Vector3 target)
    {
        return (target - t.position).normalized;
    }

    /// <summary>
    /// Explicitly set X position of transform
    /// </summary>
    /// <param name="T"></param>
    /// <param name="xPosition">Position to set</param>
    public static void SetPositionX(this Transform T, float xPosition)
    {
        T.transform.position = new Vector3(xPosition, T.transform.position.y, T.position.z);
    }

    /// <summary>
    /// Explicitly set Y position of transform
    /// </summary>
    /// <param name="T"></param>
    /// <param name="yPosition">Position to set</param>
    public static void SetPositionY(this Transform T, float yPosition)
    {
        T.transform.position = new Vector3(T.transform.position.x, yPosition, T.position.z);
    }

    /// <summary>
    /// Explicitly set Z position of transform
    /// </summary>
    /// <param name="T"></param>
    /// <param name="zPosition">Position to set</param>
    public static void SetPositionZ(this Transform T, float zPosition)
    {
        T.transform.position = new Vector3(T.transform.position.x, T.position.y, zPosition);
    }

    /// <summary>
    /// Immediately destroy all child game objects of transform.
    /// </summary>
    public static void DestroyAllChildrenImmediate(this Transform transform)
    { foreach (Transform t in transform) { UnityEngine.Object.DestroyImmediate(t.gameObject); } }


    /// <summary>
    /// Destroy all child game objects of transform.
    /// </summary>
    public static void DestroyAllChildren(this Transform transform)
    { foreach (Transform t in transform) { UnityEngine.Object.Destroy(t.gameObject); } }

    /// <summary>
    /// Move all children to a new parent
    /// </summary>
    /// <param name="parent">Transform to transfer all children to.</param>
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

    public static List<GameObject> GetAllChildren(this Transform transform)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in transform)
            children.Add(t.gameObject);
        return children;
    }

    //TODO: Look into
    public static void SetOrthographicSizeWithWidthLock(this Camera cam, float desiredHeight)
    {

        float desiredAspect = 16f / 9f;
        float aspect = cam.aspect;
        float ratio = desiredAspect / aspect;
        cam.orthographicSize = desiredHeight * ratio;
    }

}