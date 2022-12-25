using System;
using Laio;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Laio
{
    public struct ToastInput
    {
        public ToastInput(string name)
        {
            this.Name = name;
            this.Value = "";
        }

        public ToastInput(string name, string defaultValue)
        {
            this.Name = name;
            this.Value = defaultValue;
        }

        public string Name;
        public string Value;
    }

    public struct ToastButton
    {

        public ToastButton(string name)
        {
            this.Name = name;
            this.Value = 0;
        }

        public ToastButton(string name, int defaultValue)
        {
            this.Name = name;
            this.Value = defaultValue;
        }

        public string Name;
        public int Value;
    }


    public struct ToastContent
    {

        public ToastContent(string header,
            string description,
            ToastInput[] input,
            ToastButton[] buttons)
        {
            this.Header = header;
            this.Description = description;
            this.input = input;
            this.buttons = buttons;
        }

        public string Header;
        public string Description;

        public ToastInput[] input;
        public ToastButton[] buttons;

        public string[] GetInput()
        {
            string[] returnValue = new string[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                returnValue[i] = input[i].Value;
            }
            return returnValue;
        }
    }

    public delegate void OnToastSelection(string[] inputs, int button);

    public class Toast : EditorWindow
    {
        public static OnToastSelection onToastSelection;
        private static ToastContent toastContent;

        public static void ShowToast(ToastContent content)
        {
            Toast window = (Toast)EditorWindow.GetWindow(typeof(Toast));
            window.name = "Toast";
            window.maxSize = new Vector2(350, 225);
            window.minSize = new Vector2(350, 225);
            toastContent = content;
        }

        public void OnGUI()
        {
            GUILayout.Label(toastContent.Header, LaioStyle.Header);
            //GUILayout.Label(toastContent.Description, LaioStyle.WrappingText);
            GUILayout.Space(10);
            GUILayout.Box(toastContent.Description, LaioStyle.WrappingText);
            GUI.skin.button.wordWrap = false;

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            for (int i = 0; i < toastContent.input.Length; i++)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(toastContent.input[i].Name);

                toastContent.input[i].Value = GUILayout.TextField(toastContent.input[i].Value);

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (toastContent.buttons.Length != 0)
            {
                for (int i = 0; i < toastContent.buttons.Length; i++)
                {
                    if (GUILayout.Button(toastContent.buttons[i].Name))
                    {
                        onToastSelection?.Invoke(toastContent.GetInput(), toastContent.buttons[i].Value);
                        Close();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
            }
            GUILayout.EndHorizontal();

        }

    }

}
