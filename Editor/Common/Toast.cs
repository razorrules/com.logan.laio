using UnityEditor;
using UnityEngine;

namespace LaioEditor
{


    public delegate void OnToastSelection(string[] inputs, int button);

    public class Toast : EditorWindow
    {
        public OnToastSelection onToastSelection;
        private ToastContent toastContent;

        public static Toast ShowToast(ToastContent content, OnToastSelection callback = null)
        {
            var window = GetWindow<Toast>("Toast");
            window.toastContent = content;
            window.onToastSelection = callback;
            return window;
        }


        public void OnGUI()
        {
            if (toastContent.buttons == null)
            {
                Close();
                return;
            }

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
