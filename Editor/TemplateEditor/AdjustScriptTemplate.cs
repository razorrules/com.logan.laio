using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace LaioEditor.Tools
{
    /// <summary>
    /// Adjust the default script template and the ability to create custom
    /// c# templates. 
    /// 
    /// Uses ScriptTemplateSimpleText.xml to figure out all of the settings for this.
    /// Such as, grabbing a list of available using options, method options etc. This
    /// also contains the actual code for these keys.
    /// </summary>
    public class AdjustScriptTemplate : EditorWindow
    {

        private const string TOAST_DESCRIPTION =
            "Create a script template. These go in Assets/ScriptTemplates and follow the naming convention of \"{order}-{menu}_{name}-{fileName}\"\n\n" +
            "You must restart unity for these templates to appear.";

        private const string DEFAULT_FILE = "81-C# Script-NewBehaviourScript.cs.txt";

        private static readonly Regex Rgx_WhiteSpace = new Regex(@"\s+");

        internal static string XML_PATH
        {
            get
            {

                return PackagePath + "/Editor/TemplateEditor/ScriptTemplateSimpleText.xml";

            }
        }

        internal static string PackagePath
        {
            get
            {
                string data = Application.dataPath.Replace("Assets", "Library/PackageCache/");
                string full = Directory.GetDirectories(data).ToList().Where(s => s.ToLower().Contains("com.logan.laio".ToLower())).FirstOrDefault();
                return full;
            }
        }

        internal static string TemplatePath
        {
            get
            {
                return Application.dataPath + "/ScriptTemplates/";
            }
        }

        internal static string CopyTemplatePath
        {
            get
            {
                return PackagePath + "/Editor/TemplateEditor/ScriptTemplates/";
            }
        }

        private Settings _settings;
        private string _textBox;
        private static bool _firstOpen;
        private Vector2 _fileScroll;

        private static XDocument _doc;

        private List<string> _allMethods;

        private List<string> _allUsings;

        /// <summary>
        /// Get all method keys in XML document
        /// </summary>
        public List<string> AllMethodKeys
        {
            get
            {
                //If value is cached, return that, otherwise get the list.
                if (_allMethods != null)
                    return _allMethods;

                _allMethods = new List<string>();
                List<XElement> elements = xmlDoc.Descendants("method").ToList();
                //Add the list and return the value
                foreach (XElement element in elements)
                    _allMethods.Add(element.Attribute("Key").Value.Trim());
                return _allMethods;
            }
        }

        /// <summary>
        /// Get all using keys in XML document
        /// </summary>
        public List<string> AllUsingKeys
        {
            get
            {
                //If value is cached, return that, otherwise get the list.
                if (_allUsings != null)
                    return _allUsings;

                _allUsings = new List<string>();
                List<XElement> elements = xmlDoc.Descendants("using").ToList();
                //Add the list and return the value
                foreach (XElement element in elements)
                    _allUsings.Add(element.Attribute("Key").Value.Trim());
                return _allUsings;
            }
        }

        /// <summary>
        /// Get the XDocument, if it is not set, set it. 
        /// </summary>
        public static XDocument xmlDoc
        {
            get
            {
                if (_doc == null)
                    _doc = XDocument.Load(XML_PATH);
                return _doc;
            }
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Adjust Script Template")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            AdjustScriptTemplate window = (AdjustScriptTemplate)EditorWindow.GetWindow(typeof(AdjustScriptTemplate));
            //Set min size and content of window
            window.minSize = new Vector2(600, 700);
            window.titleContent.text = "Script templates";
            window.titleContent.tooltip = "Create script templates and adjust the original script template.";
            window.Show();
            selectedFile = DEFAULT_FILE;
            //Flag first open
            _firstOpen = true;
        }

        /// <summary>
        /// Sets
        /// </summary>
        public void CreateDefaultSettings()
        {
            _settings.Methods = new Dictionary<string, bool>();
            _settings.Usings = new Dictionary<string, bool>();

            foreach (string method in AllMethodKeys)
                _settings.Methods.Add(method, false);
            foreach (string use in AllUsingKeys)
                _settings.Usings.Add(use, false);

            //Default methods
            _settings.Methods["Start"] = true;
            _settings.Methods["Update"] = true;

            //Default using statements
            _settings.Usings["UnityEngine"] = true;
            _settings.Usings["System"] = true;
            _settings.Usings["System_Collections"] = true;
            _settings.Usings["System_Collections_Generic"] = true;
        }

        /// <summary>
        /// Ensure that the template folder and files exists, if not, copy them from laio template
        /// </summary>
        public void EnsureTemplateExists()
        {
            //Check if directory exists:
            if (!Directory.Exists(TemplatePath))
            {
                Directory.CreateDirectory(TemplatePath);

                foreach (string file in Directory.GetFiles(CopyTemplatePath))
                {
                    File.Copy(file, TemplatePath + Path.GetFileName(file));
                }

            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Reload data
        /// </summary>
        private void HotReload()
        {
            EnsureTemplateExists();

            try
            {
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
                //Create default settings in case there is null, and load
                CreateDefaultSettings();
                Load();
                _firstOpen = false;
            }
            catch (DirectoryNotFoundException exception)
            {
                this.Close();
            }
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }



        private void OnGUI()
        {
            try
            {
                //Check if a reload needs to take place.
                if (_settings.Methods == null || _firstOpen)
                    HotReload();
            }
            catch (FileNotFoundException exception)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            DrawHierarchy();
            //Draw options and file editor
            DrawOptions();
            GUILayout.EndHorizontal();
            DrawFile(500);

            GUILayout.BeginHorizontal();
            //Save current template
            if (GUILayout.Button("Save"))
            {
                Save();
            }

            //Create a new template
            if (GUILayout.Button("Create Template"))
            {
                Toast.ShowToast(new ToastContent("Create Template",
                    TOAST_DESCRIPTION,
                   new ToastInput[] { new ToastInput("Menu", "C# Templates"), new ToastInput("item name"), new ToastInput("file name") },
                   new ToastButton[] { new ToastButton("Cancel"), new ToastButton("Create") }
                   ));

                Toast.onToastSelection += OnToastSelection;
            }
            GUILayout.EndHorizontal();

        }

        public static string selectedFile = "Default";

        public GUIStyle hierarchy;
        public GUIStyle hierarchyNotSelected;
        public GUIStyle hierarchySelected;

        private Vector2 hierarchyScroll = Vector2.zero;
        public void DrawHierarchy()
        {
            GUILayout.BeginVertical("Box");

            if (hierarchyNotSelected == null)
            {
                hierarchyNotSelected = new GUIStyle();
                hierarchyNotSelected.normal.textColor = Color.white;

                hierarchySelected = new GUIStyle();
                hierarchySelected.normal.textColor = new Color(1, .45f, .45f);

                hierarchy = new GUIStyle();

            }
            hierarchyScroll = GUILayout.BeginScrollView(hierarchyScroll, GUILayout.Width(200));

            GUILayout.BeginVertical();
            foreach (string file in Files())
            {
                if (selectedFile.Equals(file))
                {
                    if (GUILayout.Button(ParseFileNameToReadable(file), hierarchySelected))
                        selectedFile = file;
                }
                else
                {
                    if (GUILayout.Button(ParseFileNameToReadable(file), hierarchyNotSelected))
                    {
                        selectedFile = file;
                        HotReload();
                    }
                }

            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        public string ParseFileNameToReadable(string input)
        {
            //"81-Templates__C# Script-NewBehaviourScript";

            string menu = "";
            string fileName = "";

            //Value[0] = order
            //Value[1] = menu & item
            //Value[1] = file
            string[] values = input.Split('-');

            string[] menuSplit = values[1].Replace("__", "_").Split('_');

            if (menuSplit.Length == 2)
            {
                return menuSplit[0] + "/" + menuSplit[1];
            }
            else
            {
                return values[1];
            }

        }

        private List<string> Files()
        {
            List<string> returnValue = new List<string>();
            int i = 0;
            foreach (string s in Directory.GetFiles(TemplatePath))
            {
                if (!s.Contains(".meta"))
                {
                    returnValue.Add(Path.GetFileName(s));
                    if (s.Contains(DEFAULT_FILE))
                    {
                        returnValue.Swap(0, i);
                    }
                    i++;
                }
            }


            return returnValue;
        }

        /// <summary>
        /// Called when you confirm the toast. Used to save a new template.
        /// </summary>
        /// <param name="values">Values of inputs</param>
        /// <param name="button">Button pressed</param>
        private void OnToastSelection(string[] values, int button)
        {
            // 0 is the close button, otherwise we confirmed
            if (button == 0)
                return;

            //If inputs is not 3, then probably not what we need, as we use Menu, ItemName, FileName
            if (values.Length != 3)
                throw new Exception("Error occured on ToastSelection call back. Not enough values returned.");

            //Create new name
            string newFile = $"{80}-{values[0]}__{values[1]}-{values[2]}.cs.txt"; ;

            //Create and write the file contents
            File.Create(Application.dataPath + "/ScriptTemplates/" + newFile).Close();
            File.WriteAllText(Application.dataPath + "/ScriptTemplates/" + newFile, _textBox);

            //Reimport the new file
            AssetDatabase.ImportAsset("Assets/ScriptTemplates/" + newFile);

            //Remove delegate assignment
            Toast.onToastSelection -= OnToastSelection;
        }

        /// <summary>
        /// Draw the options for easy editing.
        /// </summary>
        public void DrawOptions()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            //====== Main options
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Class Comment: [" + (_settings.classComments ? "x" : "") + "]"))
            {
                _settings.classComments = !_settings.classComments;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            //====== Methods
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Methods", LaioStyle.Header2);

            foreach (KeyValuePair<string, bool> pair in _settings.Methods.ToList())
            {
                _settings.Methods[pair.Key] = EditorGUILayout.Toggle(pair.Key, _settings.Methods[pair.Key]);
            }

            GUILayout.EndVertical();

            //====== Using
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Usings", LaioStyle.Header2);

            foreach (KeyValuePair<string, bool> pair in _settings.Usings.ToList())
            {
                _settings.Usings[pair.Key] = EditorGUILayout.Toggle(pair.Key.Replace("_", "."), _settings.Usings[pair.Key]);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //Generate text from options button
            if (GUILayout.Button("Generate"))
            {
                GenerateTextFromOptions();
            }
            GUILayout.EndVertical();

        }

        public string GetPath()
        {
            return TemplatePath + selectedFile;
        }

        /// <summary>
        /// Get a value from the XML document
        /// </summary>
        /// <param name="descendants">Descendants to look at</param>
        /// <param name="key">Key to look for</param>
        /// <returns></returns>
        public string GetValue(string descendants, string key)
        {
            return (string)xmlDoc.Descendants(descendants).ToList().Where(e => e.Attribute("Key").Value.ToLower().Equals(key.ToLower())).FirstOrDefault().Value;
        }

        /// <summary>
        /// Generates text from the options you have selected. 
        /// </summary>
        public void GenerateTextFromOptions()
        {
            StringBuilder strBuilder = new StringBuilder();

            //=== Construct using statements
            foreach (KeyValuePair<string, bool> pair in _settings.Usings)
            {
                if (pair.Value)
                    strBuilder.Append(GetValue("using", pair.Key) + "\n");
            }

            //=== Build namespace and class header.

            string comments = "";

            if (_settings.classComments)
            {
                comments = "\n" +
                    "//<Summary>\n" +
                    "//\n" +
                    "//</Summary>\n";
            }

            strBuilder.Append(
                "\n#ROOTNAMESPACEBEGIN#\n" + comments +
                "public class #SCRIPTNAME# : MonoBehaviour\n{\n");

            //=== Add methods

            foreach (KeyValuePair<string, bool> pair in _settings.Methods)
            {
                if (pair.Value)
                    strBuilder.Append(GetValue("method", pair.Key));
            }

            //=== Close string

            strBuilder.Append("\n}\n#ROOTNAMESPACEEND#");

            _textBox = strBuilder.ToString();
        }

        /// <summary>
        /// Remove al white space from a string
        /// </summary>
        /// <param name="input">String to remove white space from</param>
        /// <returns>Input with no whitespaces</returns>
        public static string RemoveWhitespace(string input)
        {
            return Rgx_WhiteSpace.Replace(input, "");
        }

        /// <summary>
        /// Read the current c# template, and auto populate the settings struct.
        /// </summary>
        public void Load()
        {
            string tokens = "\n";
            string[] text = File.ReadAllLines(GetPath());

            //String builder to build the text from the file.
            StringBuilder strBuilder = new StringBuilder();
            foreach (string str in text)
                strBuilder.Append(str + tokens);

            string loaded = RemoveWhitespace(strBuilder.ToString());

            //Load all methods
            foreach (KeyValuePair<string, bool> pair in _settings.Methods.ToList())
            {
                if (loaded.ToString().Contains(RemoveWhitespace(GetValue("method", pair.Key))))
                    _settings.Methods[pair.Key] = true;
            }

            //Load all usings
            foreach (KeyValuePair<string, bool> pair in _settings.Usings.ToList())
            {
                if (loaded.ToString().Contains(RemoveWhitespace(GetValue("using", pair.Key))))
                    _settings.Usings[pair.Key] = true;
            }
            _textBox = strBuilder.ToString();
        }

        /// <summary>
        /// Save the new template
        /// </summary>
        public void Save()
        {

            //Ensure file exsists
            File.WriteAllText(GetPath(), _textBox);
            AssetDatabase.ImportAsset("Assets/ScriptTemplates/" + selectedFile);
        }

        /// <summary>
        /// Draw the file for text edit
        /// </summary>
        /// <param name="maxHeight">maximum height of text area</param>
        public void DrawFile(float maxHeight)
        {
            //Setup scroll view, and place text area inside
            _fileScroll = EditorGUILayout.BeginScrollView(_fileScroll, GUILayout.MaxHeight(maxHeight));
            _textBox = EditorGUILayout.TextArea(_textBox);
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// struct to store the settings for the editor
        /// </summary>
        struct Settings
        {
            public bool classComments;

            public Dictionary<string, bool> Methods;
            public Dictionary<string, bool> Usings;
        }

    }
}