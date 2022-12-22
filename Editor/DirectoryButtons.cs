using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Laio.Tools
{
    struct Btn
    {
        public Btn(string name, string directory)
        {
            this.name = name;
            this.directory = directory;
        }

        public string name;
        public string directory;
    }
    public class DirectoryButtons : EditorWindow
    {
        static string drive { get { return Path.GetPathRoot(Environment.CurrentDirectory); } }

        static Btn[] buttons = new Btn[] {
            new Btn("Script Templates", $@"{drive}\Program Files\Unity\Hub\Editor\{Application.unityVersion}\Editor\Data\Resources\ScriptTemplates"),
        };

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Directory Buttons")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            DirectoryButtons window = (DirectoryButtons)EditorWindow.GetWindow(typeof(DirectoryButtons));
            window.Show();
            Vector2 size = new Vector2(205, buttons.Length * 50);
            window.minSize = size;
            window.maxSize = size;
            window.title = "Directory";
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach (Btn button in buttons)
            {
                if (GUILayout.Button(button.name, GUILayout.Height(50), GUILayout.Width(200)))
                {
                    Application.OpenURL(button.directory);
                }
            }
            GUILayout.EndVertical();
        }

    }
}