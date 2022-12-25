using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ScriptTemplateManager
{


    private static List<string> loadedTemplates;

    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MyMenu/Do Something")]
    static void DoSomething()
    {
        Debug.Log("Doing Something...");
    }

    private static string GetPath()
    {
        return Application.dataPath + "/ScriptTemplates/";
    }

    private static void LoadTemplates()
    {
        string[] files = null;

        try
        {
            files = Directory.GetFiles(GetPath());
        }
        catch (DirectoryNotFoundException)
        { /* Only exception we care about catching */ }
        finally
        {
            if (files == null || files.Length == 0)
            {
                if (CreateFolderAndFiles())
                {
                    files = Directory.GetFiles(GetPath());
                }
            }
        }

        loadedTemplates = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains(".meta"))
                continue;
            loadedTemplates.Add(files[i]);
        }

    }


    #region Copy template to root of assets

    /// <summary>
    /// Used to create the ScriptTemplates folder and populated it with Unity default templates
    /// </summary>
    /// <returns>Was creation successful?</returns>
    private static bool CreateFolderAndFiles()
    {
        Directory.CreateDirectory(GetPath());

        foreach (string s in GetPreloadedTemplates())
        {
            using (var stream = File.Create(GetPath() + Path.GetFileName(s)))
            {
                string dataasstring = GetPreloadedTemplate(s); //your data
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                stream.Write(info, 0, info.Length);
            }
        }

        return true;
    }

    /// <summary>
    /// Get a list of all preloaded templetes 
    /// </summary>
    private static string[] GetPreloadedTemplates()
    {
        List<string> files = new List<string>();

        string path = Application.dataPath + "/Laio/Tools/Editor/ScriptTemplates/";

        try
        {
            foreach (string s in Directory.GetFiles(path))
            {
                //Exclude all meta files
                if (s.Contains(".meta"))
                    continue;

                files.Add(s);
            }

        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogError("Laio directory has been changed from root of assets. Please move folder back to 'Assets/Laio'");
        }
        return files.ToArray();
    }

    /// <summary>
    /// Get contents of a template from the preloaded directory.
    /// </summary>
    /// <param name="path">Full path to file</param>
    /// <returns>Contents of file</returns>
    private static string GetPreloadedTemplate(string path)
    {
        return File.ReadAllText(path);
    }

    #endregion

}
