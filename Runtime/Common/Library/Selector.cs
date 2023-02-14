using System.Collections.Generic;

namespace Laio.Library
{
    public class Selector<T>
    {
        public delegate void OnSelectionUpdate(T deselected, T selected);
        public delegate void OnSelectedIndexUpdate(int oldSelection, int newSelection);

        public OnSelectionUpdate onSelectionUpdate;
        public OnSelectedIndexUpdate onSelectedIndexUpdate;

        private List<T> _options;

        private int _selection;

        public int SelectedIndex
        {
            get => _selection;
            set
            {
                //Ensure that the value wraps around to never go out of range with list
                int newValue = value;
                if (value >= _options.Count)
                    newValue = 0;
                if (value < 0)
                    newValue = _options.Count - 1;

                //Ensure that the values are different
                if (newValue == _selection)
                    return;
                int cache = _selection;
                _selection = newValue;

                onSelectedIndexUpdate?.Invoke(cache, _selection);
                onSelectionUpdate?.Invoke(_options[cache], _options[_selection]);

            }
        }

        public Selector()
        {
            _options = new List<T>();
        }

        public Selector(params T[] options)
        {
            _options = new List<T>(options);
        }

        public void Decrement() =>
            SelectedIndex--;

        public void Increment() =>
            SelectedIndex++;

        public void AddRange(params T[] optionRange)
        {
            _options.AddRange(optionRange);
        }

        public void AddOption(T newOption)
        {
            _options.Add(newOption);
        }

    }
}