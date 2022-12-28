using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using System.IO;
using System.Text;

public static class Saving
{

    /// <summary>
    /// Saves the scriptable obejct as a JSON to persistent data path.
    /// </summary>
    public static void Save(object objectToSave, string name, string path = "/Save/", string extension = ".txt")
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
    private static object LoadObject(Type type, string name, string path = "/Save/", string extension = ".txt")
    {
        object returnObj = Activator.CreateInstance(type);

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
    public static T Load<T>(string name, string path = "/Save/", string extension = ".txt")
    {
        return (T)LoadObject(typeof(T), name, path, extension);
    }


    //========================  ScriptableObjects  ======================== //
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
    private static ScriptableObject LoadScriptableObject(Type type, string name, string path = "/Save/", string extension = ".txt")
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

    /// <summary>
    /// Load a scriptable object
    /// </summary>
    public static void Load(this ScriptableObject obj, string name, string path = "/Save/", string extension = ".txt")
    {
        obj = LoadScriptableObject(obj.GetType(), name, path, extension);
    }
}