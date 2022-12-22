using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Laio
{

    public static class LaioStyle
    {

        static Font font = Font.CreateDynamicFontFromOSFont("Trebuchet MS Bold Italic", 16);

        static GUIStyle _header;



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
                }
                return _header;
            }
        }

        static GUIStyle _header1;
        static GUIStyle _header2;
        static GUIStyle _header3;

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