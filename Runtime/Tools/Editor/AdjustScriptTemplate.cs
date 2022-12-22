using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Laio;
using System.IO;
using System.Linq;

namespace Laio.Tools
{
    public enum Using
    {
        Unity_Editor, System, System_Collections, System_Collections_Generic, TMPro, System_Threading_Tasks,
    }

    public enum Method
    {
        //Basic
        Awake, Start, Update, LateUpdate, OnEnable, OnDisable,
        //Other, more uncommon
        OnTriggerEnter, OnTriggerExit, OnCollisionEnter, OnCollisionExit,
        OnDrawGizmos, OnGUI
    }

    public class AdjustScriptTemplate : EditorWindow
    {
        static Settings settings;

        static GUISkin skin;

        static string textBox;
        bool drawSimpleLayout = true;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Adjust Script Template")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            AdjustScriptTemplate window = (AdjustScriptTemplate)EditorWindow.GetWindow(typeof(AdjustScriptTemplate));
            window.Show();
        }

        private void OnGUI()
        {

            if (GUILayout.Button("Load"))
            {
                GUI.skin = Resources.Load("GUI.guiskin") as GUISkin;
                Load();
            }

            int methodsLength = Enum.GetValues(typeof(Method)).Length;
            if (settings.methods == null || settings.methods.Length != methodsLength)
            {
                settings.methods = new bool[methodsLength];
            }
            int usingLength = Enum.GetValues(typeof(Using)).Length;
            if (settings.Using == null || settings.Using.Length != usingLength)
            {
                settings.Using = new bool[usingLength];
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Select edit mode");
            if (GUILayout.Button("Simple Edit"))
            {
                drawSimpleLayout = true;
            }
            if (GUILayout.Button("Custom"))
            {
                drawSimpleLayout = false;
            }
            EditorGUILayout.EndHorizontal();

            if (drawSimpleLayout)
            {
                DrawBasic();
            }
            else
            {
                DrawAdvanced();
            }


        }

        public void DrawBasic()
        {
            GUILayout.Label("Options: ", LaioStyle.Header);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Class Comments: [" + (settings.classComments ? "x" : "") + "]"))
            {
                settings.classComments = !settings.classComments;
            }
            if (GUILayout.Button("Method Comments: [" + (settings.methodComments ? "x" : "") + "]"))
            {
                settings.methodComments = !settings.methodComments;
            }
            GUILayout.Label("NameSpace: ");

            settings.nameSpace = GUILayout.TextArea(settings.nameSpace);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Methods Implemented", LaioStyle.Header2);
            for (int i = 0; i < settings.methods.Length; i++)
            {
                settings.methods[i] = EditorGUILayout.Toggle(Enum.GetName(typeof(Method), i), settings.methods[i]);
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Usings included", LaioStyle.Header2);
            for (int i = 0; i < settings.Using.Length; i++)
            {
                settings.Using[i] = EditorGUILayout.Toggle(Enum.GetName(typeof(Using), i), settings.Using[i]);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
            {
                Save();
            }
        }

        //foreach (var file in Directory.GetFiles(path))
        //    {
        //        Console.WriteLine(System.IO.Path.GetFileName(file)); // file name
        //    }

        //string directory = Environment.CurrentDirectory;
        //string drive = Path.GetPathRoot(directory);
        //drive = drive.Remove(2);
        //string path = $@"{drive}/Program Files/Unity/Hub/Editor/{Application.unityVersion}/Editor/Data/Resources/ScriptTemplates/";
        //Debug.Log(path);
        public string GetPath()
        {
            string fileName = "81-C# Script-NewBehaviourScript";
            return Application.dataPath + "/ScriptTemplates/" + fileName + ".cs.txt";
        }

        public void Load()
        {
            ScriptTemplateManager.GetLoadedTemplates();

            string[] lines = File.ReadAllLines(GetPath());


        }

        public void Save()
        {

        }

        public void DrawAdvanced()
        {
            if (GUILayout.Button("Generate based off basic"))
            {

            }
            textBox = EditorGUILayout.TextArea(textBox, GUILayout.Height(500));

            if (GUILayout.Button("Save"))
            {
                Save();
            }
        }


        struct Settings
        {
            public bool methodComments;

            public bool classComments;

            public string nameSpace;

            public bool[] Using;
            public bool[] methods;

        }
    }
}