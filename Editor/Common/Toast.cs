using UnityEditor;
using UnityEngine;

namespace LaioEditor
{


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
