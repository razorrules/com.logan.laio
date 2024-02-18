using System;
using System.Collections;
using Laio;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    /// Look in a given direction
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="movementDirection"></param>
    public static void LookInDirection(this Transform transform, Vector3 movementDirection)
    {
        Vector3 lookAtPosition = transform.position + movementDirection;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    /// <summary>
    /// Get a random point in bounds. Does not handle rotation. 
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns>Random point in bounds</returns>
    public static Vector3 RandomPointInBounds(this Bounds bounds)
    {
        System.Random random = new System.Random();
        return new Vector3()
        {
            x = LaioMath.FromBlend(random.NextDouble(), bounds.min.x, bounds.max.x),
            y = LaioMath.FromBlend(random.NextDouble(), bounds.min.y, bounds.max.y),
            z = LaioMath.FromBlend(random.NextDouble(), bounds.min.z, bounds.max.z)
        };
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
    /// Rotate a given vector point around a origin
    /// </summary>
    /// <param name="point">Point to be rotated</param>
    /// <param name="pivot">Origin of the rotation</param>
    /// <param name="rotation">Rotation to apply</param>
    /// <returns>Rotated point</returns>
    public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        return rotation * (point - pivot) + pivot;
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
    /// Checks if an index is within the range of the array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index">Index to check</param>
    /// <returns>Is the index valid?</returns>
    public static bool ValidIndex<T>(this IList<T> list, int index)
    {
        if (index < 0 || index >= list.Count)
            return false;
        return true;
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
    /// Get all child objects of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static List<GameObject> GetAllChildren(this Transform transform)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in transform)
            children.Add(t.gameObject);
        return children;
    }

    //private static RaycastResult HoveringObject(this GraphicRaycaster graphicsRaycaster, out RaycastHit)
    //{
    //    //Set up the new Pointer Event
    //    PointerEventData data = new PointerEventData(EventSystem.current);
    //    //Set the Pointer Event Position to that of the game object
    //    data.position = Input.mousePosition;

    //    //Create a list of Raycast Results
    //    List<RaycastResult> results = new List<RaycastResult>();

    //    //Raycast using the Graphics Raycaster and mouse click position
    //    graphicsRaycaster.Raycast(data, results);

    //    return results[0];
    //}

    //private static bool AllHoveringObject(this GraphicRaycaster graphicsRaycaster, out List<RaycastResult> results)
    //{
    //    //Set up the new Pointer Event
    //    PointerEventData data = new PointerEventData(EventSystem.current);
    //    //Set the Pointer Event Position to that of the game object
    //    data.position = Input.mousePosition;

    //    //Raycast using the Graphics Raycaster and mouse click position
    //    rayCaster.Raycast(data, results);

    //    return results;
    //}

}