using UnityEngine;

namespace LaioEditor
{
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

            if (buttons != null)
                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].Value = i;
            else
                Debug.LogError("ToastContent.buttons is null! Toast Content requires at least 1 button.");
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


        public string Name;
        public int Value;
    }

}
