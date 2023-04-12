using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laio.Tools
{

    public enum GCommandDisplayMode
    {
        None,
        Single,
        Full
    }

    public class GCommandInterface : MonoBehaviour
    {

        public const string CONTROL_NAME = "Input";

        public KeyCode InterfaceKey = KeyCode.Tilde;

        private bool isShowing;
        private GCommandDisplayMode _displayMode;
        private string _input;

        private void Update()
        {
            if (Input.GetKeyDown(InterfaceKey))
            {
                _displayMode = Helper.Next(_displayMode);
                StartCoroutine(DelayFrameFocus());
            }
        }

        public System.Collections.IEnumerator DelayFrameFocus()
        {
            yield return null;
            GUI.FocusControl(CONTROL_NAME);
        }

        public void OnGUI()
        {
            if (_displayMode == GCommandDisplayMode.Single)
            {
                _input = GUILayout.TextField(_input, GUILayout.Width(Screen.width), GUILayout.Height(50));
                GUI.SetNextControlName(CONTROL_NAME);
            }
        }
    }
}
