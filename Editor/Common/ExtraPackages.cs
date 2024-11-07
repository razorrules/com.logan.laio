using LaioEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

public class ExtraPackages : EditorWindow
{
    private struct ExtraPackage
    {
        public string Name;
        public string GitURL;
        public string Description;
    }
    static AddRequest Request;

    private ExtraPackage[] Packages = new ExtraPackage[]
    {
        new ExtraPackage(){
            Name = "EasyButtons",
            GitURL= "https://github.com/madsbangh/EasyButtons.git#upm",
            Description= "Adds the ability to quickly show a button in the inspector for any method.",
        },
        new ExtraPackage(){
            Name = "Scene-ref-attribute",
            GitURL= "https://github.com/KyleBanks/scene-ref-attribute.git",
            Description= "Unity C# attribute for serializing component and interface references within the scene or prefab during OnValidate, rather than using GetComponent* functions in Awake/Start/OnEnable at runtime.",
        },
    };

    private GUIStyle _githubStyle;

    [MenuItem("Tools/More Packages")]
    public static void ShowExample()
    {
        ExtraPackages window = GetWindow<ExtraPackages>();
        window.minSize = new Vector2(450, 300);
        window.maxSize = new Vector2(450, 400);
        window.titleContent.text = "More Packages";
        window.titleContent.tooltip = "More Github packages that are recommended to install.";
        window.Show();

    }

    private void CreateStyles()
    {
        _githubStyle = new GUIStyle();
        _githubStyle.normal.textColor = Color.cyan;
    }


    private void OnGUI()
    {
        if (_githubStyle == null)
            CreateStyles();

        foreach (ExtraPackage ep in Packages)
            DrawPackage(ep);
    }

    private void DrawPackage(ExtraPackage extraPackage)
    {
        DrawPackage(extraPackage.Name, extraPackage.GitURL, extraPackage.Description);
    }

    private void DrawPackage(string name, string gitUrl, string desc)
    {
        GUILayout.BeginVertical("Box");
        GUILayout.Label(name, LaioStyle.Header);
        if (GUILayout.Button(gitUrl, _githubStyle))
        {
            Application.OpenURL(gitUrl);
        }
        GUILayout.Space(10);
        GUILayout.Label(desc, LaioStyle.WrappingText);
        GUILayout.Space(5);

        if (GUILayout.Button("Install", GUILayout.Width(100)))
        {
            Request = Client.Add(gitUrl);
            EditorApplication.update += Progress;
        }
        GUILayout.EndVertical();
    }

    static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
                Debug.Log("Installed: " + Request.Result.packageId);
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            EditorApplication.update -= Progress;
        }
    }

}

