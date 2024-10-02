using UnityEngine;

namespace LaioEditor
{

    public static class LaioStyle
    {

        private static Font font = Font.CreateDynamicFontFromOSFont("Arial", 12);

        private static GUIStyle _header;
        private static GUIStyle _header1;
        private static GUIStyle _header2;
        private static GUIStyle _header3;
        private static GUIStyle _wrappingText;

        public static GUIStyle WrappingText
        {
            get
            {
                if (_wrappingText == null)
                {
                    _wrappingText = new GUIStyle();
                    _wrappingText.wordWrap = true;
                    _wrappingText.normal.textColor = Color.white;
                    _wrappingText.font = font;
                }
                return _wrappingText;
            }
        }

        public static GUIStyle Header
        {
            get
            {
                if (_header == null)
                {
                    _header = new GUIStyle();
                    _header.normal.textColor = Color.white;
                    _header.fontSize = 20;
                    _header.font = font;
                    _header.fontStyle = FontStyle.Bold;
                }
                return _header;
            }
        }

        public static GUIStyle Header1
        {
            get
            {
                if (_header1 == null)
                {
                    _header1 = new GUIStyle();
                    _header1.normal.textColor = Color.white;
                    _header1.fontSize = 16;
                    _header1.font = font;
                }
                return _header1;
            }
        }

        public static GUIStyle Header2
        {
            get
            {
                if (_header2 == null)
                {
                    _header2 = new GUIStyle();
                    _header2.normal.textColor = Color.white;
                    _header2.fontSize = 14;
                    _header2.font = font;
                }
                return _header2;
            }
        }

        public static GUIStyle Header3
        {
            get
            {
                if (_header3 == null)
                {
                    _header3 = new GUIStyle();
                    _header3.normal.textColor = Color.white;
                    _header3.fontSize = 12;
                    _header3.font = font;
                }
                return _header3;
            }
        }

    }

}